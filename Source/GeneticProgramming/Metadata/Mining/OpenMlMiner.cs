using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using GeneticProgramming.Data.Dao;
using GeneticProgramming.Data.Models;
using Metadata.Global;
using Metadata.Import;
using Metadata.Serialization;
using Newtonsoft.Json;
using OpenML;
using OpenML.Response;
using DatasetMetadata = GeneticProgramming.Data.Models.DatasetMetadata;

namespace Metadata.Mining
{
    public class OpenMlMiner
    {
        private readonly OpenMlConnector _openMlConnector = new OpenMlConnector("3c287fc383aae0144a1787a29f0fd890");
        private readonly IDbEntitiesProvider _dbEntitiesProvider=new DbEntitiesProvider(false);

        public List<Dataset> GetAvailableDatasets()
        {
            var datasets=_openMlConnector.ListDatasets();
            return datasets;
        }

        public DatasetDescription GetDatasetDescription(int datasetId)
        {
            return _openMlConnector.GetDatasetDescription(datasetId);
        }

        public bool CanDatasetBeDownloaded(DatasetDescription datasetDetails)
        {
            if (string.IsNullOrEmpty(datasetDetails.Url))
            {
                return false;
            }
            return string.Equals(datasetDetails.Licence, "public", StringComparison.InvariantCultureIgnoreCase);
        }

        public List<int> GetDatasetOpenMlIdsInDatabase()
        {
            return _dbEntitiesProvider.DatasetsIds();
        }

        public List<DatasetMetadata> GetDatasetsInDatabase()
        {
            return _dbEntitiesProvider.GetMetadatas();
        }

        /// <summary>
        /// Adds missing openml qualities as a columns to the metadata tables
        /// </summary>
        public void AddMissingDataQualities()
        {
            var datasetQualities = _openMlConnector.ListDataQualities();
            foreach (var datasetQuality in datasetQualities)
            {
                var qualityWithoutDots = datasetQuality.Replace(".", "_");
                if (!_dbEntitiesProvider.ColumnExistInTable("Metadata", qualityWithoutDots))
                {
                    _dbEntitiesProvider.AddColumnToTable("Metadata", qualityWithoutDots);
                }
            }
        }

        public void SaveDataQualities()
        {
            var datasetsInDatabase = GetDatasetOpenMlIdsInDatabase();
            foreach (var openMlId in datasetsInDatabase)
            {
                var datasetGlobalMetadata = _openMlConnector.GetDatasetQualities(openMlId);
                if (datasetGlobalMetadata == null)
                {
                    //TODO: what to do
                    continue;
                }
                foreach (var metadata in datasetGlobalMetadata)
                {
                    string destinationColumn = metadata.Name.Replace(".","_");
                    double destinationValue = Double.Parse(metadata.Value, CultureInfo.InvariantCulture);
                    string whereClause = "OpenMlId = "+openMlId;
                    _dbEntitiesProvider.SetValue("Metadata", destinationColumn, destinationValue, whereClause);
                }
            }
        }

        /// <summary>
        /// Checks for dataset not matching via hash, either updated or deleted in openml, sets DeletedInOpenMl flag accordingly
        /// </summary>
        public void UpdateDeletedInOpenMl()
        {
            var datasetsInDatabase = GetDatasetsInDatabase();
            foreach (var dataset in datasetsInDatabase)
            {
                if (dataset.DeletedInOpenMl)
                {
                    continue;
                }
                var datasetGlobalMetadata = _openMlConnector.GetDatasetDescription(dataset.OpenMlId);
                bool deleted = false;
                if (datasetGlobalMetadata == null)
                {
                    deleted = true;
                }
                else if (datasetGlobalMetadata.Md5CheckSum != dataset.Hash)
                {
                    deleted = true;
                }
                else if (datasetGlobalMetadata.Name != dataset.DatasetName)
                {
                    deleted = true;
                }
                if (deleted)
                {
                    _dbEntitiesProvider.SetValue("Metadata", "DeletedInOpenMl", 1, "OpenMlId = "+dataset.OpenMlId);
                }
            }
        }

        public void SaveRunsForDataset(int datasetId,HashSet<int> runsInDb)
        {
           var result= _openMlConnector.ExecuteFreeQuery("SELECT * FROM input_data d " +
                "JOIN run r " +
                "on d.run = r.rid " +
                "JOIN algorithm_setup a " +
                "ON r.setup=a.sid " +
                "WHERE data = "+datasetId);
            var data=result.Data;
            foreach (var run in data)
            {
                int rid = int.Parse(run[3]);
                Console.WriteLine("Processing rid "+rid);
                if (runsInDb.Contains(rid)) continue;
                var openRun=new OpenMlRun();
                openRun.OpenMlRunId = rid;
                openRun.TaskId = int.Parse(run[7]);
                string algoWithParams=run[23];
                var firstSpace=algoWithParams.IndexOf(' ');
                string alg = algoWithParams;
                string parameters = "";
                if (firstSpace != -1)
                {
                    alg = algoWithParams.Substring(0, firstSpace);
                    parameters = algoWithParams.Substring(firstSpace);
                }
                openRun.Algorithm = alg;
                openRun.Parametres = parameters;
                openRun.DatasetId = datasetId;
                var runDetails= _openMlConnector.GetRun(rid);
                if (runDetails == null)
                {
                    Console.WriteLine("rid missing "+rid);
                    continue;
                }
                if (runDetails.OutputData==null || runDetails.OutputData.Evaluations == null) continue;
                if (runDetails.OutputData.Evaluations.Any(x => x.Name == "predictive_accuracy"))
                {
                    var first = runDetails.OutputData.Evaluations.First(x => x.Name == "predictive_accuracy");
                    openRun.PredictiveAccuracy = Double.Parse(first.Value, CultureInfo.InvariantCulture);
                    try
                    {
                    _dbEntitiesProvider.SaveRun(openRun);

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    Console.WriteLine("Saved rid " + rid);
                }
                string b = "";
            }
        }

        public void UpdateRuns(HashSet<int> runsInDb)
        {
            foreach (var rid in runsInDb)
            {

                var runDetails = _openMlConnector.GetRun(rid);
                if (runDetails == null)
                {
                    Console.WriteLine("rid missing " + rid);
                    continue;
                }
                if (runDetails.OutputData == null || runDetails.OutputData.Evaluations == null) continue;

                var openRun = new OpenMlRun();
                openRun.OpenMlRunId = rid;
                bool update = false;
                if (runDetails.OutputData.Evaluations.Any(x => x.Name == "usercpu_time_millis"))
                {
                    update = true;
                    var first = runDetails.OutputData.Evaluations.First(x => x.Name == "usercpu_time_millis");
                    openRun.UserCpuTimeMillis = Double.Parse(first.Value, CultureInfo.InvariantCulture);
                }

                if (runDetails.OutputData.Evaluations.Any(x => x.Name == "usercpu_time_millis_testing"))
                {
                    update = true;
                    var first = runDetails.OutputData.Evaluations.First(x => x.Name == "usercpu_time_millis_testing");
                    openRun.UserCpuTimeMillisTesting = Double.Parse(first.Value, CultureInfo.InvariantCulture);
                }

                if (runDetails.OutputData.Evaluations.Any(x => x.Name == "usercpu_time_millis_training"))
                {
                    update = true;
                    var first = runDetails.OutputData.Evaluations.First(x => x.Name == "usercpu_time_millis_training");
                    openRun.UserCpuTimeMillisTraining = Double.Parse(first.Value, CultureInfo.InvariantCulture);
                }

                if (runDetails.OutputData.Evaluations.Any(x => x.Name == "run_cpu_time"))
                {
                    update = true;
                    var first = runDetails.OutputData.Evaluations.First(x => x.Name == "run_cpu_time");
                    openRun.RunCpuTime = Double.Parse(first.Value, CultureInfo.InvariantCulture);
                }

                if (runDetails.OutputData.Evaluations.Any(x => x.Name == "build_cpu_time"))
                {
                    update = true;
                    var first = runDetails.OutputData.Evaluations.First(x => x.Name == "build_cpu_time");
                    openRun.BuildCpuTime = Double.Parse(first.Value, CultureInfo.InvariantCulture);
                }

                if (runDetails.OutputData.Evaluations.Any(x => x.Name == "mean_absolute_error"))
                {
                    update = true;
                    var first = runDetails.OutputData.Evaluations.First(x => x.Name == "mean_absolute_error");
                    openRun.MeanAbsoluteError = Double.Parse(first.Value, CultureInfo.InvariantCulture);
                }
                try
                {
                    if (update)
                    {
                        _dbEntitiesProvider.UpdateRun(openRun);
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                Console.WriteLine("Updated rid " + rid);

                string b = "";
            }
        }


        public void ExtractMetadataForDataset(string fileLocation,string datasetName, string hash, int openMlId)
        {
            var serializer = new SharpMetadataSerializer();
            var importer = new CsvMetadataImporter(new AttributeAnalyzer());
                try
                {
                    var currentmet = importer.ImportMetadata(fileLocation);
                    var serialized = serializer.SerializeToJson(currentmet);
                    var metadata = new DatasetMetadata
                    {
                        DatasetName = datasetName,
                        Hash = hash,
                        OpenMlId = openMlId,
                        AttributeMetadata = serialized,
                        IsCorrupted = false,
                        IsDisabled = false,
                        NumberOfAttributes = currentmet.AttributesCount
                    };
                    _dbEntitiesProvider.UpsertMetadata(metadata);
                }
                catch (Exception e)
                {
                    dynamic error = new { error = e.Message,stackTrace=e.StackTrace };
                    var serialized = JsonConvert.SerializeObject(error); 
                    var metadata = new DatasetMetadata
                    {
                        DatasetName = datasetName,
                        Hash = hash,
                        OpenMlId = openMlId,
                        AttributeMetadata = serialized,
                        IsCorrupted = true,
                        IsDisabled = false,
                        NumberOfAttributes = 0
                    };
                    _dbEntitiesProvider.UpsertMetadata(metadata);
                    Trace.WriteLine("Failed to parse: " + fileLocation);
                }
        }

        public bool DatasetMetadataExist(int datasetId)
        {
            return _dbEntitiesProvider.DatasetMetadataExist(datasetId);
        }

        public HashSet<int> GetRunsInDatabase()
        {
            return _dbEntitiesProvider.GetRunsInDatabase();
        }
    }
}
