using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using GeneticProgramming.Core.Fitness;
using GeneticProgramming.Core.GpClient;
using GeneticProgramming.Core.GPService;
using GeneticProgramming.Core.Helpers;
using GeneticProgramming.Data.Contracts;
using GeneticProgramming.Data.Dao;
using log4net;
using Metadata.Distance.Kernelization;
using Metadata.Import;
using Metadata.Prediction.Evaluation;
using Metadata.Results;

namespace GeneticProgramming.ComputationNode.Tasks
{
    public class FitnessComputationTask:IComputationTask
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(FitnessComputationTask));
        private static int _numberOfThreads;
        private static readonly List<Task> Tasks =new List<Task>();
        private static bool _cancellationPending;
        private static readonly DbEntitiesProviderFactory DbEntitiesProviderFactory=new DbEntitiesProviderFactory();
        private static FitnessEvaluator _evaluator;

        public FitnessComputationTask(int numberOfThreads)
        {
            _numberOfThreads = numberOfThreads;
        }

        public static async Task ComputeFitnessAsync(int threadNr)
        {
            var task= new Task(() => ComputeFitness(threadNr));
            task.Start();
            await task;
        }

        private static void LogResultFinished(IndividualEvaluationResults evaluationResults, int threadNr)
        {
            Logger.Info(String.Format("Thread {0} finished computation: {1}", threadNr, evaluationResults));
        }

        private static void LogStartComputation(ProgramEnvelope programEnvelope, int threadNr)
        {
             Logger.Info(String.Format("Thread {0} is starting computation: {1}", threadNr, programEnvelope));
        }

        private static void ComputeFitness(int threadNr)
        {
            var client = GpClientHelpers.GetClient();
            while (!_cancellationPending)
            {
                try
                {
                    ProgramEnvelope p = client.GetIndividual();
                    LogStartComputation(p, threadNr);
                    MainFitness(p, client, threadNr);
                }
                catch (EndpointNotFoundException)
                {
                    Console.WriteLine("Server not responding. Thread {0} reconnecting soon", threadNr);
                    Thread.Sleep(30000);
                    Console.WriteLine("Thread {0} reconnecting", threadNr);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message + ". " + e.StackTrace);
                    Thread.Sleep(60000);
                }
            }
        }

        private static void MainFitness(ProgramEnvelope p, GPServiceClient client, int threadNr)
        {
            var results = _evaluator.Evaluate(p);
            foreach (var result in results)
            {
                LogResultFinished(result, threadNr);
                client.RateIndividual(result);
            }
        }

        private static void CreateTasks()
        {
            var provider = DbEntitiesProviderFactory.CreateDbEntitiesProvider();
            var globalMetadataInclusion = new GlobalMetadataSettings(GlobalMetadataInclusion.DontInclude);
            var metadataImporter = new DbMetadataImporter(provider, globalMetadataInclusion);
            var resultsProvider = new ResultsProvider(provider);
            var qualityEvaluator = new RankingPredictorEvaluator(metadataImporter, resultsProvider);
            var fitnessFunction = new AttributeAlignmentFitness(qualityEvaluator, new NoKernelization(),null, 17 ,false, false);
            _evaluator = new FitnessEvaluator(fitnessFunction);
            Logger.Info("Computing fitness");
            for (int i = 0; i < _numberOfThreads; i++)
            {
                var task = ComputeFitnessAsync(i);
                Tasks.Add(task);
            }
        }

        public void Execute()
        {
            using (var dbProvider =DbEntitiesProviderFactory.CreateDbEntitiesProvider())
            {

                string startMessage = String.Format("Starting fitness computation - number of threads is {0}, hostname is {1}",
                    _numberOfThreads,
                    Environment.MachineName);
                Console.WriteLine(startMessage);
                dbProvider.LogMessageAsync(startMessage);
                CreateTasks();
                while (!_cancellationPending)
                {
                    string s = Console.ReadLine();
                    if (s == "q")
                    {
                        _cancellationPending = true;
                    }
                    else if (s == "print")
                    {
                        Console.WriteLine("Number of running tasks: " + Tasks.Count(x=>x.Status==TaskStatus.Running));
                    }
                    else
                    {
                        Console.WriteLine("You have to write q to exit");
                        Thread.Sleep(5000);
                    }
                }
                Console.WriteLine("Obtained the command to exit, waiting for all threads to finish their cycle.");
                Task.WaitAll(Tasks.ToArray());
            }
        }
    }
}
