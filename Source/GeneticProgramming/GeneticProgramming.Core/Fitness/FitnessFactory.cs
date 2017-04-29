using System;
using GeneticProgramming.Data.Dao;
using Metadata.Distance.HungarianAlgorithm;
using Metadata.Distance.Kernelization;
using Metadata.Import;
using Metadata.Prediction.Evaluation;
using Metadata.Results;
using Newtonsoft.Json.Linq;

namespace GeneticProgramming.Core.Fitness
{
    public static class FitnessFactory
    {
        public static IFitness CreateFitness(FitnessTypes fitnessType, DbEntitiesProviderFactory dbEntitiesProviderFactory, JToken config, int numberOfNeigbours, bool multiobjective)
        {
            switch (fitnessType)
            {
                case FitnessTypes.TestFitness:
                    return new TestFitness();
                case FitnessTypes.AttributeAlignmentFitness:
                    Console.WriteLine("Initializing global opt fitness");
                    var dbEntitiesProvider = dbEntitiesProviderFactory.CreateDbEntitiesProvider();
                    var globalMetadaMetadataSettings = new GlobalMetadataSettings(GlobalMetadataInclusion.DontInclude);
                    var importer = new DbMetadataImporter(dbEntitiesProvider, globalMetadaMetadataSettings);
                    Console.WriteLine("Initializing metadata importer");
                    var resultsProvider = new ResultsProvider(dbEntitiesProvider);
                    var qualityEvaluator = new RankingPredictorEvaluator(importer, resultsProvider);
                    var repairToSemimetric = config.Value<bool>("RepairToSemimetric");
                    var repairToAttributeSemimetric = config.Value<bool>("RepairToAttributeSemimetric");
                    var factory = new DummyDistanceSettings(config, qualityEvaluator.MetadataImporter.Metadata).DummyDistanceFactory;
                    return new AttributeAlignmentFitness(qualityEvaluator, new NoKernelization(), factory, numberOfNeigbours , repairToSemimetric, repairToAttributeSemimetric,
                        false, multiobjective);
            }
            return null;
        }
    }
}
