using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Dapper;
using GeneticProgramming.Data.Contracts;
using GeneticProgramming.Data.Models;

namespace GeneticProgramming.Data.Dao
{
    public class DbEntitiesProvider: IDbEntitiesProvider
    {
        private List<BestResult> _bestResults;
        private List<MetadataWithResults> _metadataWithResults;

        private readonly DbConnectionProvider _connectionProvider;

        private DbConnection Connection { get; set; }

        public DbEntitiesProvider(bool readOnly,bool useSqlLite=true)
        {
            _connectionProvider = new DbConnectionProvider(useSqlLite, readOnly);
            Connection=_connectionProvider.GetConnection();
            Connection.Open();
        }

        public bool ColumnExistInTable(string tableName, string columnName, string schema = "pikater")
        {
            string query = string.Format(@"SELECT COUNT(*) FROM information_schema.COLUMNS WHERE TABLE_SCHEMA = '{0}' AND TABLE_NAME = '{1}' 
                AND COLUMN_NAME = '{2}'", schema, tableName, columnName);
            var queryCount = Connection.ExecuteScalar<int>(query);
            return queryCount > 0;
        }

        public void AddColumnToTable(string tableName, string columnName)
        {
            string query = String.Format("ALTER TABLE pikater.{0} ADD `{1}` FLOAT NULL ;", tableName,
                columnName);
            Connection.Execute(query);
        }

        public void SetValue(string tableName, string columnName, double value, string whereClause)
        {
            string query = String.Format("UPDATE pikater.{0} SET `{1}`=@a WHERE {2}", tableName, columnName, whereClause);
            Connection.Execute(query,  new { a = value});
        }

        public Dictionary<int, GlobalDatasetMetadata> GetGlobalMetadata(HashSet<string> filter)
        {
            var columnNames = GlobalMetadataColumns.GetFilteredColumns(filter);
            var query ="SELECT `Metadata`.`Id`," +
   String.Join(",",columnNames.Select(columnName=> "`Metadata`.`"+columnName+"`"))+
@"FROM `Metadata` WHERE 
    `Metadata`.`IsCorrupted` = 0 AND
    `Metadata`.`IsDisabled` = 0 AND
    `Metadata`.`DeletedInOpenMl` = 0 AND
    kNN_3NKappa is not null;";
            var reader = Connection.ExecuteReader(query);
            var result = new Dictionary<int, GlobalDatasetMetadata>();
            while (reader.Read())
            {
                int id;
                if (reader[0] is long) id = (int) (long) reader[0];
                else id = (int) reader[0];
                var values = new List<float>();
                for (int i = 1; i < columnNames.Count+1; i++)
                {
                    float currentValue;
                    if (reader[i] is float) currentValue = (float) reader[i];
                    else currentValue = (float)(double)reader[i];
                    values.Add(currentValue);
                }
                var currentGlobal = new GlobalDatasetMetadata {Id = id, Values = values};
                result.Add(id,currentGlobal);

            }
            return result;
        }

        public void SaveExperimentResults(string name,string result, string type, string settings)
        {
            string sourceName =  Environment.MachineName;
            Connection.Execute(@"insert INTO ExperimentResults (Name, Settings, HostName, Type, Result , CreatedOn)  values (@a, @f, @b, @c, @d, @e)",
                new { a = name, b = sourceName, c=type, d = result, e = DateTime.Now, f = settings });
        }

        public string GetSettingsForHost(string hostName)
        {
            var result =Connection.Query<string>(@"SELECT SETTINGS FROM ExperimentSettings WHERE HostName = @a",
                new { a = hostName });
            var rows = result as string[] ?? result.ToArray();
            if (rows.Length > 0)
            {
                return rows.First();
            }
            return "";
        }

        public List<BestResult> BestResults
        {
            get
            {
                lock (_connectionProvider)
                {
                    if (_bestResults == null)
                    {
                        Console.WriteLine("Getting best results:" + Connection);
                        _bestResults =
                            Connection.Query<BestResult>("SELECT * FROM BestResults", null, null, false, 50000)
                                .Where(x => !x.AgentType.ToLower().Contains("boost") &&
                                            !x.AgentType.ToLower().Contains("bagging") &&
                                            !x.AgentType.ToLower().Contains("stacking") &&
                                            !x.AgentType.ToLower().Contains("rotation")).ToList();
                    }
                    Console.WriteLine("Obtained best results:" + Connection);
                    return _bestResults;
                }
            }
        }

        public List<MetadataWithResults> MetadataWithResults
        {
            get
            {
                lock (_connectionProvider)
                {
                    if (_metadataWithResults == null)
                    {
                        _metadataWithResults = Connection.Query<MetadataWithResults>("SELECT * FROM MetadataWithResults", null, null, false, 50000).ToList();
                    }
                    return _metadataWithResults;    
                }
            }
        }

        public void LogMessageAsync(string message, string experimentName=null, string source=null)
        {
            string sourceName = source ?? Environment.MachineName;
            Connection.ExecuteAsync(@"insert INTO Log (Source,Experiment, Message, CreatedAt)  values (@a, @b, @c, @d)",
                new { a = sourceName, b = experimentName, c = message, d = DateTime.Now });


        }

        public void LogMessage(string message, string experimentName = null, string source=null)
        {
            string sourceName = source ?? Environment.MachineName;
            Connection.Execute(@"insert INTO Log (Source,Experiment, Message, CreatedAt)  values (@a, @b, @c, @d)",
                new { a = sourceName, b = experimentName, c = message, d = DateTime.Now });

        }

        public bool DatasetMetadataExist(int datasetId)
        {
            var res=Connection.ExecuteScalar<int>("SELECT COUNT(*) FROM Metadata WHERE OpenMlId = @did",
                new {did=datasetId});
            return res > 0;
        }

        public void UpsertMetadata(DatasetMetadata metadata)
        {
            Connection.Execute(@"DELETE FROM Metadata WHERE Hash=@c;insert into Metadata (DatasetName,Hash,OpenMlId,AttributeMetadata, IsCorrupted, IsDisabled, NumberOfAttributes) 
values (@b,@c,@d,@e,@f,@g,@h);",
                new
                {
                    b = metadata.DatasetName,
                    c = metadata.Hash,
                    d = metadata.OpenMlId,
                    e = metadata.AttributeMetadata,
                    f = metadata.IsCorrupted,
                    g = metadata.IsDisabled,
                    h = metadata.NumberOfAttributes
                });
        }

        public void UpdateAttributes(int openMlId, string attributesMetadata)
        {
            var result = Connection.Execute(@"UPDATE Metadata SET AttributeMetadata = @b WHERE OpenMlId=@a;",
                new
                {
                    a = openMlId,
                    b = attributesMetadata
                });
            if (result != 1)
            {
                throw new Exception();
            }
        }

        public List<int> DatasetsIds()
        {
            return Connection.Query<int>("SELECT OpenMlId FROM Metadata WHERE IsCorrupted = 0 AND IsDisabled = 0 AND DeletedInOpenMl = 0 AND kNN_3NKappa is not null", null, null, false, 50000).ToList();
        }

        public List<DatasetMetadata> GetMetadatas()
        {
            return Connection.Query<DatasetMetadata>("SELECT * FROM Metadata WHERE IsCorrupted = 0 AND IsDisabled = 0  AND DeletedInOpenMl = 0 AND kNN_3NKappa is not null", null, null, false, 50000).ToList();
        }

        public ExperimentControl GetExperimentControl(string experimentName)
        {
            var results =
                Connection.Query<ExperimentControl>("SELECT * FROM ExperimentControl WHERE ExperimentName = @name",
                    new {name = experimentName}, null, false, 50000);
            if (!results.Any()) return null;
            return results.First();
        }

        public void UpdateRun(OpenMlRun openRun)
        {
            Connection.Execute(@"UPDATE OpenMLRun SET 
                UserCpuTimeMillis=@c,
                UserCpuTimeMillisTesting = @d,
                UserCpuTimeMillisTraining = @e,
                RunCpuTime = @f,
                BuildCpuTime = @g,
                MeanAbsoluteError = @h 
                WHERE OpenMlRunId=@b",
                new
                {
                    b = openRun.OpenMlRunId,
                    c = openRun.UserCpuTimeMillis,
                    d = openRun.UserCpuTimeMillisTesting,
                    e = openRun.UserCpuTimeMillisTraining,
                    f = openRun.RunCpuTime,
                    g = openRun.BuildCpuTime,
                    h = openRun.MeanAbsoluteError
                });
        }

        public HashSet<int> GetRunsInDatabase()
        {
            var res= Connection.Query<int>("SELECT OpenMlRunId FROM OpenMLRun", null, null, false, 50000).ToList();
            var toReturn = new HashSet<int>();
            foreach (var re in res)
            {
                toReturn.Add(re);
            }
            return toReturn;
        }

        public void SaveRun(OpenMlRun openRun)
        {
            Connection.Execute(@"Insert into OpenMLRun (OpenMlRunId,DatasetId,Algorithm,Parametres, TaskId,PredictiveAccuracy) 
            values (@b,@c,@d,@e,@f,@g);",
                new
                {
                    b = openRun.OpenMlRunId,
                    c = openRun.DatasetId,
                    d = openRun.Algorithm,
                    e = openRun.Parametres,
                    f = openRun.TaskId,
                    g=openRun.PredictiveAccuracy
                });
        }

        public void Dispose()
        {
            Connection.Dispose();
        }
    }
}
