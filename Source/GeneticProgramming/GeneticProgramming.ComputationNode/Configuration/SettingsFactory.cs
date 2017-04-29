using System;
using GeneticProgramming.ComputationNode.Configuration.ExperimentSettings;
using GeneticProgramming.ComputationNode.Tasks.Subtasks;
using log4net;
using Newtonsoft.Json.Linq;

namespace GeneticProgramming.ComputationNode.Configuration
{
    public class SettingsFactory
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(SettingsFactory));
        public static ISettings ParseSettings(JToken config)
        {
            var type = config.Value<string>("Type");
            Logger.Info(string.Format("Creating experiment settings from {0}",type));
            var subtaskType = (SubtaskTypes)Enum.Parse(typeof(SubtaskTypes), type);
            switch (subtaskType)
            {
                case SubtaskTypes.EvolutionOptimization:
                    return new EvolutionOptSettings(config);
                case SubtaskTypes.Baseline:
                    return new BaselineSettings();
                case SubtaskTypes.MasterThesisAlignment:
                    return new MasterThesisAlignmentSettings(config);
                case SubtaskTypes.GeneticProgramming:
                    return new GeneticProgrammingSettings(config);
            }
            return null;
        }
    }
}
