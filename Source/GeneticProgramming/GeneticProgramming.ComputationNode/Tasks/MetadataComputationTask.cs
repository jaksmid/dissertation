using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GeneticProgramming.Data.Dao;
using Metadata.Mining;

namespace GeneticProgramming.ComputationNode.Tasks
{
    public class MetadataComputationTask : IComputationTask
    {
        private static int _numberOfThreads;
        private static readonly List<Task> Tasks =new List<Task>();
        private static bool _cancellationPending;
        private static readonly DbEntitiesProviderFactory DbEntitiesProviderFactory=new DbEntitiesProviderFactory();
        private static bool _recomputeMetadata = false;

        public MetadataComputationTask(int numberOfThreads)
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
            using (var dbProvider = DbEntitiesProviderFactory.CreateDbEntitiesProvider())
            {
                var miner = new OpenMlMiner();
                var datasets = miner.GetAvailableDatasets();
                var rnd = new Random();
                string savePath = "Datasets\\" + Environment.MachineName + "\\Thread" + threadNr + "\\";
                if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);
                //Delete everything from the previous run: 
                var downloadedMessageInfo = new DirectoryInfo(savePath);
                foreach (FileInfo file in downloadedMessageInfo.GetFiles())
                {
                    file.Delete();
                }
                while (datasets.Count > 0)
                {
                    var currentDataset = datasets[rnd.Next(datasets.Count)]; //new Dataset() { Id = 583, Name = "nki70" };
                    Console.WriteLine("Examining dataset "+currentDataset.Name);
                    datasets.Remove(currentDataset);
                    Console.WriteLine("Obtaining further details");
                    var datasetDetails = miner.GetDatasetDescription(currentDataset.Id);
                    Console.WriteLine("Obtained");
                    if (miner.CanDatasetBeDownloaded(datasetDetails))
                    {
                        Console.WriteLine("Dataset " + currentDataset.Name + " public");
                        if (!datasetDetails.Format.EndsWith("arff",StringComparison.InvariantCultureIgnoreCase)) continue;
                        if (_recomputeMetadata || !miner.DatasetMetadataExist(currentDataset.Id))
                        {
                            try
                            {
                            string message = "Thread " + threadNr + ": Extracting metadata for " + currentDataset.Name;
                            Console.WriteLine(message);
                            dbProvider.LogMessageAsync(message);
                            //save file
                            string saveLoc = savePath + currentDataset.Name;
                            using (var myWebClient = new ExtendedWebClient())
                            {
                                if (!saveLoc.EndsWith("arff", StringComparison.InvariantCultureIgnoreCase))
                                    saveLoc = saveLoc + ".arff";
                                //myWebClient.OpenRead(datasetDetails.Url);
                                //Int64 bytes_total = Convert.ToInt64(myWebClient.ResponseHeaders["Content-Length"]);
                                myWebClient.DownloadFile(datasetDetails.Url, saveLoc);
                                if ((new FileInfo(saveLoc).Length) > 10000000)
                                {
                                    Console.WriteLine("Dataset " + currentDataset.Name + " too large");
                                    File.Delete(saveLoc);
                                    continue;
                                }
                                
                            }
                            //extract metadata
                            miner.ExtractMetadataForDataset(saveLoc,datasetDetails.Name,datasetDetails.Md5CheckSum,datasetDetails.Id);
                            message = "Thread " + threadNr + ": Extracted metadata for " + currentDataset.Name;
                            Console.WriteLine(message);
                            dbProvider.LogMessageAsync(message);
                            File.Delete(saveLoc);
                            }
                            catch (Exception e)
                            {
                                string errorMessage="Error extracting metadata"+e.Message+" "+e.StackTrace+" "+e;
                                Console.WriteLine(e);
                                dbProvider.LogMessageAsync(errorMessage);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Dataset " + currentDataset.Name + " metadata already in database");    
                        }
                    }
                    else
                    {
                        Console.WriteLine("Dataset " + currentDataset.Name+" not public");
                    }
                }
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