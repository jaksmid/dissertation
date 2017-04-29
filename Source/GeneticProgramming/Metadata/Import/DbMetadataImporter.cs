using System;
using System.Collections.Generic;
using System.Linq;
using GeneticProgramming.Data.Dao;
using GeneticProgramming.Data.Models;
using Metadata.Global;
using Metadata.Normalization;
using Metadata.Serialization;
using DatasetMetadata = Metadata.Global.DatasetMetadata;

namespace Metadata.Import
{
    public class DbMetadataImporter
    {
        public GlobalMetadataSettings GlobalMetadataSettings { get; set; }
        private readonly IDbEntitiesProvider _entities;
        private readonly SharpMetadataSerializer _serializer=new SharpMetadataSerializer();
        public MetadataCollection Metadata { get; set; }

        public DbMetadataImporter(IDbEntitiesProvider entities, GlobalMetadataSettings globalMetadataSettings)
        {
            GlobalMetadataSettings = globalMetadataSettings;
            _entities = entities;

            var availableMetadata = _entities.MetadataWithResults;
            if (GlobalMetadataSettings.GlobalMetadataInclusion == GlobalMetadataInclusion.DontInclude)
            {
                var metadataCollection =
                    availableMetadata.Select(metadataWithResults => _serializer.SerializeFromJson(metadataWithResults))
                        .ToList();
                Metadata = new MetadataCollection(metadataCollection);
            }
            else
            {
                var globalMetadata = _entities.GetGlobalMetadata(GlobalMetadataSettings.Filter);
                if (GlobalMetadataSettings.GlobalMetadataInclusion == GlobalMetadataInclusion.IncludeAndNormalize)
                {
                    NormalizeGlobalMetadata(globalMetadata);
                }
                var collection = availableMetadata.Select(
                    metadataWithResults =>
                        new Tuple<DatasetMetadata, GlobalDatasetMetadata>(_serializer.SerializeFromJson(metadataWithResults), globalMetadata.ContainsKey(metadataWithResults.Id) ? globalMetadata[metadataWithResults.Id] : null))
                    .ToList();
                foreach (var tuple in collection)
                {
                    tuple.Item1.GlobalMetadata = tuple.Item2;
                }
                Metadata = new MetadataCollection(collection.Select(x => x.Item1));
            }
        }

        public MetadataCollection GetMetadata(HashSet<string> positiveFilter = null)
        {
            if (positiveFilter != null)
            {
                var toReturn = new MetadataCollection()
                {
                    Metadatas = Metadata.Metadatas.Where(metadata => positiveFilter.Contains(metadata.Name)).ToList()
                };
                return toReturn;
            }
            return Metadata;
        }

        public void NormalizeGlobalMetadata(Dictionary<int, GlobalDatasetMetadata> metadata)
        {
            int globalMetadataCount = metadata.First().Value.Values.Count;
            for (int i = 0; i < globalMetadataCount; i++)
            {
                List<float> values = metadata.Values.Select(x => x.Values[i]).ToList();
                var transformation = Normalizations.CreateTransformationFunction(values);
                foreach (var globalDatasetMetadata in metadata.Values)
                {
                    globalDatasetMetadata.Values[i] = transformation(globalDatasetMetadata.Values[i]);
                }
            }
        }
    }
}
