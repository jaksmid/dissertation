using System;
using System.Collections.Generic;
using GeneticProgramming.Data.Contracts;
using GeneticProgramming.Data.Models;

namespace GeneticProgramming.Data.Dao
{
    public interface IDbEntitiesProvider : IDisposable
    {
        List<BestResult> BestResults { get; }
        List<MetadataWithResults> MetadataWithResults { get; }
        void LogMessageAsync(string message, string experimentName=null, string source=null);
        void LogMessage(string message, string experimentName = null, string source=null);

        bool DatasetMetadataExist(int datasetId);

        void UpsertMetadata(DatasetMetadata metadata);

        void UpdateAttributes(int openMlId, string attributesMetadata);

        List<int> DatasetsIds();
        HashSet<int> GetRunsInDatabase();
        void SaveRun(OpenMlRun openRun);

        List<DatasetMetadata> GetMetadatas();

        ExperimentControl GetExperimentControl(string experimentName);
        void UpdateRun(OpenMlRun openRun);

        bool ColumnExistInTable(string tableName, string columnName, string schema = "pikater");

        void AddColumnToTable(string tableName, string columnName);

        void SetValue(string tableName, string columnName, double value, string whereClause);

        Dictionary<int, GlobalDatasetMetadata> GetGlobalMetadata(HashSet<string> filter);

        void SaveExperimentResults(string name, string result, string type, string settings);

        string GetSettingsForHost(string hostName);
    }
}