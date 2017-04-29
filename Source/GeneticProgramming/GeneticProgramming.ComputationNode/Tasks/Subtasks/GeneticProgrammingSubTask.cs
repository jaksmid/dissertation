using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GeneticProgramming.ComputationNode.Configuration.ExperimentSettings;
using GeneticProgramming.Core.GpClient;
using GeneticProgramming.Server.Core;
using log4net;
using Metadata.Import;

namespace GeneticProgramming.ComputationNode.Tasks.Subtasks
{
    public class GeneticProgrammingSubTask: BaseSubTask
    {
        protected static readonly ILog Logger = LogManager.GetLogger(typeof(GeneticProgrammingSubTask));

        public GeneticProgrammingSettings Settings { get; set; }
        private static readonly List<Task> Tasks = new List<Task>();
        public IGeneticProgramming Server { get; set; }
        public FitnessEvaluator Evaluator { get; set; }

        public GeneticProgrammingSubTask(GeneticProgrammingSettings settings) :
            base(new GlobalMetadataSettings(GlobalMetadataInclusion.DontInclude))
        {
            Settings = settings;
            Server = new LocalGeneticProgramming(settings);
            if (Settings.CoevolutionSettings.UseCoevolution)
            {
                Logger.Info("Starting evaluation using coevolution");
                Evaluator = new CoevolutionFitnessEvaluator(settings.FitnessFunction, Server.GetSpecificIndividual);
            }
            else
            {
                Evaluator = new FitnessEvaluator(settings.FitnessFunction);
            }
        }

        public async Task ComputeFitnessAsync(int threadNr)
        {
            var task = new Task(() => ComputeFitness(threadNr));
            task.Start();
            await task;
        }

        private void ComputeFitness(int threadNr)
        {
            var individual = Server.GetIndividual();
            while (individual != null)
            {
                var toRate = Evaluator.Evaluate(individual);
                foreach (var result in toRate)
                {
                    //The evaluation can actually rate more individual due to coevolution
                    Server.RateIndividual(result);
                }
                individual = Server.GetIndividual();
            }
        }

        public override string RunExperimentWithOpenConnection()
        {
            Logger.Info(String.Format("Starting evaluation using {0} threads", Settings.NumberOfThreads));
            for (int i = 0; i < Settings.NumberOfThreads; i++)
            {
                var task = ComputeFitnessAsync(i);
                Tasks.Add(task);
            }
            Task.WaitAll(Tasks.ToArray());
            Logger.Info(String.Format("Finished evaluation using {0} threads", Settings.NumberOfThreads));
            return null;
        }
    }
}
