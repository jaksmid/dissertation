using System;
using System.Collections.Generic;
using GeneticProgramming.ComputationNode.Configuration.ExperimentSettings.SubSettings;
using GeneticProgramming.ComputationNode.Tasks.Subtasks;
using GeneticProgramming.Core.Fitness;
using GeneticProgramming.Core.Fitness.Evolution;
using GeneticProgramming.Data.Dao;
using Metadata.Distance.HungarianAlgorithm;
using Metadata.Distance.Kernelization;
using Metadata.Import;
using Metadata.Prediction.Evaluation;
using Newtonsoft.Json.Linq;

namespace GeneticProgramming.ComputationNode.Configuration.ExperimentSettings
{
    public class EvolutionOptSettings : BaseSettings
    {
        public IKernelization Kernelization { get; set; }

        public List<KernelizationTypes> KernelizationTypes { get; set; }
        public int Generations { get; set; }

        public int Runs { get; set; }
        public IKnnSettings KnnSettings { get; set; }
        public IPOfNormSettings POfNormSettings { get; set; }
        public IPopulationSizeSettings PopulationSizeSettings { get; set; }
        public GlobalMetadataSettings GlobalMetadataSettings { get; set; }
        

        public EvolutionOptSettings(JToken config): base(config)
        {
            Generations = config.Value<int>("Generations");
            Logger.Debug("Parsed generations:" + Generations);
            Runs = config.Value<int>("Runs");
            KnnSettings = new KnnSettings(config);
            POfNormSettings = new POfNormSettings(config);
            PopulationSizeSettings = new PopulationSizeSettings(config);
            Logger.Debug("Parsed Runs:" + Runs);
            var kernelizationTuple = ConfigParser.ParseKernelization(Config);
            KernelizationTypes = kernelizationTuple.Item1;
            Kernelization = kernelizationTuple.Item2;
            Logger.Debug("Created kernelization");
            var includeGlobal = config.Value<string>("GlobalMetadata");
            Logger.Debug("Parsed includeGlobal:" + includeGlobal);
            var globalMetadataInclusion = (GlobalMetadataInclusion) Enum.Parse(typeof (GlobalMetadataInclusion), includeGlobal);
            var filterRange = new List<Tuple<string, string>>();
            var excluded = config.Value<string>("Excluded");
            string[] sep = {","};
            var splitted = excluded.Split(sep,StringSplitOptions.RemoveEmptyEntries);
            foreach (var s in splitted)
            {
                var range = s.Split('-');
                filterRange.Add(new Tuple<string, string>(range[0], range[1]));
            }
            var filter = GlobalMetadataColumns.CreateFilter(filterRange);
            GlobalMetadataSettings = new GlobalMetadataSettings(globalMetadataInclusion, filter);
        }
      

        protected string GetBody(string run)
        {
            return String.Format("_{0}_Gens{1}_{2}"
                , run, Generations, String.Join(";",KernelizationTypes));
        }

        public override string CreateExperimentName()
        {
            return BuildExperimentName(GetBody("BatchOf" + Runs));
        }

        public override ISubTask CreateSubtask()
        {
            return new EvolutionOptimizationSubTask(this);
        }

        public override string ExperimentPrefix { get { return Config.Value<string>("Fitness"); } }


        public string CreateRunName(int runId)
        {
            return BuildExperimentName(GetBody("Run" + runId));
        }

        public BaseEvolutionFitness CreateEvolutionFitness(RankingPredictorEvaluator evaluator)
        {
            var fitness = Config.Value<string>("Fitness");
            switch (fitness)
            {
                case "GlobalMetadataFitness":
                    return new GlobalMetadataFitness(evaluator, Kernelization, KnnSettings.K, POfNormSettings.P);
                case "EvolutionAlignmentFitness":
                    return new EvolutionAlignmentFitness(evaluator, KnnSettings.K, POfNormSettings.P, new DummyDistanceSettings(Config, evaluator.MetadataImporter.Metadata).DummyDistanceFactory);
                case "GlobalLocalCombined":
                    return new GlobalLocalCombined(evaluator, Kernelization, KnnSettings.K, POfNormSettings.P, new DummyDistanceSettings(Config, evaluator.MetadataImporter.Metadata).DummyDistanceFactory);
            }
            return null;
        }
    }
}
