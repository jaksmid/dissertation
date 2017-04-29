using System;
using System.Collections.Generic;
using GeneticProgramming.ComputationNode.Configuration.ExperimentSettings.GenProgramming;
using GeneticProgramming.Core.Fitness;
using GeneticProgramming.Server.Core.Settings;
using log4net;
using Metadata.Distance.Kernelization;
using Newtonsoft.Json.Linq;

namespace GeneticProgramming.ComputationNode.Configuration
{
    public class ConfigParser
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ConfigParser));

        public int ParseNumberOfThreads(JToken config)
        {
            if (config["Threads"] == null)
            {
                return Environment.ProcessorCount;
            }
            var parsedValue = config.Value<int>("Threads");
            if (parsedValue == 0)
            {
                return Environment.ProcessorCount;
            }
            return parsedValue;
        }

        public FitnessTypes ParseFitnessType(JToken config)
        {
            var type = config.Value<string>("Fitness");
            var kernelizationType = (FitnessTypes)Enum.Parse(typeof(FitnessTypes), type);
            Logger.Debug("Parsed fitness:" + type);
            return kernelizationType; 
        }

        public KernelizationTypes ParseKernelizationType(string type)
        {
            var kernelizationType = (KernelizationTypes)Enum.Parse(typeof(KernelizationTypes), type);
            Logger.Debug("Parsed kernelization:" + type);
            return kernelizationType;
        }

        public int ParsePopulationSize(JToken config)
        {
            return config.Value<int>("PopulationSize");
        }

        public List<GeneticOperatorTemplate> ParseGeneticOperatorTemplates(JToken config)
        {
            var toReturn = config["GeneticOperators"].ToObject<List<GeneticOperatorTemplate>>();
            return toReturn;
        }

        public List<PopulationTemplate> ParsePopulationsCount(JToken config)
        {
            var toReturn = config["Populations"].ToObject<List<PopulationTemplate>>();
            return toReturn;
        }

        public Tuple<List<KernelizationTypes>, IKernelization> ParseKernelization(JToken config)
        {
            var kernelizationTypes = new List<KernelizationTypes>();
            Object kernelizationObj = config["Kernelization"];
            if (kernelizationObj != null)
            {
                if (kernelizationObj is JArray)
                {
                    foreach (var kernelizationType in (JArray)kernelizationObj)
                    {
                        kernelizationTypes.Add(ParseKernelizationType(kernelizationType.Value<string>()));
                    }
                }
                else if (kernelizationObj is JValue)
                {
                    var kernelization = ((JValue)kernelizationObj).Value<string>();
                    kernelizationTypes.Add(ParseKernelizationType(kernelization));
                }
            }
            if (kernelizationTypes.Count == 0) kernelizationTypes.Add(KernelizationTypes.NoKernelization);
            return new Tuple<List<KernelizationTypes>, IKernelization>(kernelizationTypes, KernelizationFactory.CreateKernelization(kernelizationTypes));
        }
    }
}
