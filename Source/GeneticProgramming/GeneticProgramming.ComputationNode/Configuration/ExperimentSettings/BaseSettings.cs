using System;
using GeneticProgramming.ComputationNode.Tasks.Subtasks;
using log4net;
using Newtonsoft.Json.Linq;

namespace GeneticProgramming.ComputationNode.Configuration.ExperimentSettings
{
    public abstract class BaseSettings:ISettings
    {
        protected static readonly ILog Logger = LogManager.GetLogger(typeof(BaseSettings));
        protected ConfigParser ConfigParser = new ConfigParser();

        public JToken Config { get; set; }

        protected BaseSettings(JToken config)
        {
            Config = config;
            StartTicks = DateTime.Now.Ticks;
            NumberOfThreads = ConfigParser.ParseNumberOfThreads(config);
        }

        protected BaseSettings()
        {
            
        }

        public int NumberOfThreads { get; set; }

        private long StartTicks { get; set; }
        protected string BuildExperimentName(string body)
        {
            return String.Format("{0}_{1}{2}_{3}"
                , StartTicks, ExperimentPrefix, body, Environment.MachineName);
        }

        public virtual string CreateExperimentName()
        {
           return BuildExperimentName("");
        }
        public abstract ISubTask CreateSubtask();
        public abstract string ExperimentPrefix { get; }
    }
}
