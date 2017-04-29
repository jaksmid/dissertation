using System;
using System.Linq;
using Accord.Genetic;
using AForge;
using AForge.Math.Random;
using GeneticProgramming.ComputationNode.Configuration;
using GeneticProgramming.ComputationNode.Configuration.ExperimentSettings;
using GeneticProgramming.Core.Fitness;
using GeneticProgramming.Data.Reporting;
using log4net;

namespace GeneticProgramming.ComputationNode.Tasks.Subtasks
{
    public class EvolutionOptimizationSubTask: BaseSubTask
    {
        public EvolutionOptSettings Setting { get; set; }

        public EvolutionOptimizationSubTask(EvolutionOptSettings setting):base(setting.GlobalMetadataSettings)
        {
            Setting = setting;
        }

        private static readonly ILog Logger = LogManager.GetLogger(typeof(EvolutionOptimizationSubTask));
        private string RunExperiment(int experimentNumber, BaseEvolutionFitness fitness)
        {
            Random rnd = new Random();
            string runName = Setting.CreateRunName(experimentNumber);
            Logger.Info("Running experiment " + runName);
            var reporter = new GenerationProgressReporter(runName);
            var chromosomeGenerator = new UniformGenerator(new Range(0, 2), rnd.Next());
            var additiongenerator = new UniformGenerator(new Range(-2, 2), rnd.Next());
            var multiplierGenerator = new UniformGenerator(new Range(0, 2), rnd.Next());
            // create genetic population
            var chromosome = new DoubleArrayChromosome(chromosomeGenerator, multiplierGenerator, additiongenerator,
                fitness.GetChromosomesCount(Setting.GlobalMetadataSettings.Filter.Count));
            var selection = new EliteSelection();
            Logger.Info("Creating Population");
            var population = new Population(Setting.PopulationSizeSettings.PopulationSize,
                chromosome,
                fitness,
                selection);
            Logger.Info("Running first epoch");
            for (int generation = 1; generation <= Setting.Generations; generation++)
            {
                // run one epoch of the population
                population.RunEpoch();
                // check current best fitness
                var bestFitness = population.FitnessMax;
                var evaluationFitness = fitness.Validate(population.BestChromosome);
                var generationProgress = new CommonGenerationProgress(bestFitness, evaluationFitness);
                reporter.AddGeneration(generationProgress);
                Logger.Info(String.Format("Experiment {3}: Evaluated generation {0}. Max fitness is {1}; Evaluation of best is {2}", generation,
                    bestFitness, evaluationFitness, experimentNumber));
            }
            var bestIndivid = (DoubleArrayChromosome)population.BestChromosome;
            var bestInString = bestIndivid.Value.Select(x => x.ToGBString());
            string bestToString = "[" + String.Join(",", bestInString) + "]";
            var result = reporter.EndExperiment(bestToString);
            Logger.Info("Finished experiment " + experimentNumber);
            FileUtils.WriteAsFile(result, Config.ResultsDirectory + "temp_" + runName);
            SaveResultToDb.SaveResult(runName, result, "run", Setting.Config.ToString());
            return result;
        }


        public override string RunExperimentWithOpenConnection()
        {
            var fitness = Setting.CreateEvolutionFitness(QualityEvaluator);
            var source = Enumerable.Range(1, Setting.Runs).ToList();
            var result = "";
            foreach (var run in source)
            {
                result += RunExperiment(run, fitness)+",";
            }
            return result;
        }
    }
}
