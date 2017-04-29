using System.Collections.Generic;
using System.IO;
using GeneticProgramming.ComputationNode.Configuration;
using GeneticProgramming.ComputationNode.Configuration.ExperimentSettings;
using GeneticProgramming.ComputationNode.Tasks.Subtasks;
using GeneticProgramming.Data.Reporting;
using log4net;
using Newtonsoft.Json.Linq;

namespace GeneticProgramming.ComputationNode.Tasks
{
    public class AutonomousComputationTask : IComputationTask
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(AutonomousComputationTask));
        private readonly List<ISettings> _experimentSettings = new List<ISettings>();
        private readonly string _resultsDirectory = Config.ResultsDirectory;

        public AutonomousComputationTask(string configFile, bool fromFile = true)
        {
            Logger.Info("Creating AutonomousComputationTask");
            JArray config;
            if (fromFile)
            {
                Logger.Debug("Config filename is " + configFile);
                config = Utils.DeserializeJSONFromFile(configFile);
            }
            else
            {
                config = Utils.DeserializeJSON(configFile);
            }
            Logger.Info("Config is " + config);
            foreach (var setting in config)
            {
                Logger.Debug("Reading settings");
                var current = SettingsFactory.ParseSettings(setting);
                _experimentSettings.Add(current);
            }
            Logger.Debug("Created AutonomousComputationTask");
        }

        public void ExecuteSettings(ISettings setting)
        {
            string experimentName = setting.CreateExperimentName();
            Logger.Info("Executing subtask "+experimentName);
            ISubTask experiment = setting.CreateSubtask();
            string result = experiment.RunExperiment();
            if (!string.IsNullOrEmpty(result))
            {
                //TODO: get rid if this schyzophreny, let the worker decide how to report (e.g. client or server report)
                bool savedSucceeded = SaveResultToDb.SaveResult(experimentName, result, "experiment", "");
                if (!savedSucceeded)
                {
                    FileUtils.WriteAsFile(result, _resultsDirectory + experimentName);
                }
            }
        }

        public void Execute()
        {
            Logger.Info("Executing AutonomousComputationTask");
            if (!Directory.Exists(_resultsDirectory))
            {
                Directory.CreateDirectory(_resultsDirectory);
            }
            foreach (var experimentSetting in _experimentSettings)
            {
                ExecuteSettings(experimentSetting);
            }
        }
    }
}
