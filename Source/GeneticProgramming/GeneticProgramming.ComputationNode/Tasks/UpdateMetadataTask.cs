using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GeneticProgramming.Data.Dao;
using log4net;
using Metadata.Global;
using Metadata.Import;
using Metadata.Serialization;
using Newtonsoft.Json;

namespace GeneticProgramming.ComputationNode.Tasks
{
    public class UpdateMetadataTask : IComputationTask
    {
        private static int _numberOfThreads;
        private static readonly List<Task> Tasks =new List<Task>();
        private static bool _cancellationPending;
        private static readonly DbEntitiesProviderFactory DbEntitiesProviderFactory=new DbEntitiesProviderFactory();
        protected static readonly ILog Logger = LogManager.GetLogger(typeof(UpdateMetadataTask));

        public UpdateMetadataTask(int numberOfThreads)
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
            var provider = DbEntitiesProviderFactory.CreateDbEntitiesProvider(true, false);
            var metadata = provider.GetMetadatas();
            int i = 0;
            foreach (var datasetMetadata in metadata)
            {
                string searchPattern = datasetMetadata.DatasetName + "_" + datasetMetadata.OpenMlId + "_*.*";
                var di = new DirectoryInfo(@"C:\DissertationDatasets");
                FileInfo[] files = di.GetFiles(searchPattern);
                if (files.Count() != 1)
                {
                    string expectedfilename = datasetMetadata.DatasetName + "_" + datasetMetadata.OpenMlId + "_" +
                                              datasetMetadata.Hash + ".arff";
                    throw new Exception();
                }

                var attributes = JsonConvert.DeserializeObject<DatasetMetadata>(datasetMetadata.AttributeMetadata,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    });
                i++;
                //if (attributes.Attributes.First().ValuesCount > 0)
                //{
                //    Logger.Info(datasetMetadata.DatasetName+" already updated");
                //    continue;
                //}
                var serializer = new SharpMetadataSerializer();
                var importer = new CsvMetadataImporter(new AttributeAnalyzer());
                var currentmet = importer.ImportMetadata(files[0].FullName);
                var serialized = serializer.SerializeToJson(currentmet);
                //datasetMetadata.NumberOfAttributes = attributes.AttributesCount;
                provider.UpdateAttributes(datasetMetadata.OpenMlId,serialized);
                Logger.Info(datasetMetadata.DatasetName + " saved. Remaining "+(metadata.Count-i));
            }
        }

        public void Execute()
        {
            //using (var dbProvider =DbEntitiesProviderFactory.CreateDbEntitiesProvider(true, false))
            //{

            string startMessage =
                String.Format("Starting metadata computation - number of threads is {0}, hostname is {1}",
                    _numberOfThreads,
                    Environment.MachineName);
            Console.WriteLine(startMessage);
            //dbProvider.LogMessageAsync(startMessage);
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
                    Console.WriteLine("Number of running tasks: " + Tasks.Count(x => x.Status == TaskStatus.Running));
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