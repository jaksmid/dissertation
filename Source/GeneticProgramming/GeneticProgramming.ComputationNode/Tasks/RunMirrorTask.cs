using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GeneticProgramming.Data.Dao;
using Metadata.Mining;

namespace GeneticProgramming.ComputationNode.Tasks
{
    public class RunMirrorTask : IComputationTask
    {
        private static int _numberOfThreads;
        private static readonly List<Task> Tasks =new List<Task>();
        private static bool _cancellationPending;
        private static readonly DbEntitiesProviderFactory DbEntitiesProviderFactory=new DbEntitiesProviderFactory();
        private static bool _recomputeMetadata = false;
        private static bool _updateExisting = true;

        public RunMirrorTask(int numberOfThreads)
        {
            _numberOfThreads = numberOfThreads;
        }

        private static void CreateTasks()
        {
            for (int i = 0; i < _numberOfThreads; i++)
            {

                var task=ComputeMetadataAsync(i);
                Tasks.Add(task);
            }
        }

        public static async Task ComputeMetadataAsync(int threadNr)
        {
            var task= new Task(() => ComputeMetadata(threadNr));
            task.Start();
            await task;
        }

        private static void ComputeMetadata(int threadNr)
        {
            var miner = new OpenMlMiner();
            var datasetsInDb = miner.GetDatasetOpenMlIdsInDatabase();
            var runsInDb = miner.GetRunsInDatabase();
            if (!_updateExisting)
            {
                var rnd = new Random(threadNr + DateTime.Now.Millisecond);
                while (datasetsInDb.Count > 0)
                {
                    var currentIndex = rnd.Next(datasetsInDb.Count);
                    var current = datasetsInDb[currentIndex];
                    Console.WriteLine("Processing dataset " + current);
                    datasetsInDb.RemoveAt(currentIndex);
                    miner.SaveRunsForDataset(current, runsInDb);
                    runsInDb = miner.GetRunsInDatabase();
                }
            }
            else
            {
                miner.UpdateRuns(runsInDb);
            }
        }

        public void Execute()
        {
            using (var dbProvider =DbEntitiesProviderFactory.CreateDbEntitiesProvider())
            {

                string startMessage = String.Format("Starting metadata computation - number of threads is {0}, hostname is {1}",
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