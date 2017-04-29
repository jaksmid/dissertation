using System;
using System.Linq;
using GeneticProgramming.ComputationNode.Tasks;
using GeneticProgramming.Data.Reporting;
using log4net;

namespace GeneticProgramming.ComputationNode
{
    internal class Program
    {
        private static void Main(string[] arguments)
        {
            log4net.Config.XmlConfigurator.Configure();
            ILog log = LogManager.GetLogger(typeof(Program));
            log.Info(String.Format("Starting computation. Number of arguments: {0}. Arguments: {1}",
                arguments.Count(), string.Join(" ", arguments)));
            //TODO: let settings decide number of threads if the parallelization is needed
            int _numberOfThreads=8;
            IComputationTask _computationTask;
            if (arguments.Length > 0)
                {
                    var nrOfTasks = arguments[0];
                    if (!int.TryParse(nrOfTasks, out _numberOfThreads))
                    {
                        log.Error("First argument must be a number - thread count you want to allocate for the computation.");
                        return;
                    }
                }
            if (arguments.Length > 1)
            {
                var taskNr = arguments[1];
                if (taskNr == "4")
                {
                    if (arguments.Length > 2)
                    {
                        _computationTask = new AutonomousComputationTask(arguments[2] + ".json");
                        _computationTask.Execute();
                    }
                    else
                    {
                        string settings = SaveResultToDb.GetSettingsForComputer();
                        while (!string.IsNullOrEmpty(settings) && settings != "end")
                        {
                            _computationTask = new AutonomousComputationTask(settings, false);
                            _computationTask.Execute();
                            settings = SaveResultToDb.GetSettingsForComputer();
                        }
                    }
                }
                else
                {
                    if (taskNr == "0")
                    {
                        _computationTask = new RunMirrorTask(_numberOfThreads);
                    }
                    else if (taskNr == "1")
                    {
                        _computationTask = new MetadataComputationTask(_numberOfThreads);
                    }
                    else if (taskNr == "3")
                    {
                        _computationTask = new MetadataCopyTask();
                    }
                    else if (taskNr == "5")
                    {
                        _computationTask = new UpdateMetadataTask(_numberOfThreads);
                    }
                    else if (taskNr == "6")
                    {
                        _computationTask = new FitnessComputationTask(_numberOfThreads);
                    }
                    else
                    {
                        throw new Exception("Arguments out of range");
                    }
                    _computationTask.Execute();
                }
            }
        }
    }
}