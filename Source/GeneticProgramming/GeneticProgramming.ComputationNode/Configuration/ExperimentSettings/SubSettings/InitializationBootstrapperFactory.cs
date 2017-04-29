using GeneticProgramming.Server.Core.Initialization;
using Newtonsoft.Json.Linq;

namespace GeneticProgramming.ComputationNode.Configuration.ExperimentSettings.SubSettings
{
    public class InitializationBootstrapperFactory
    {
        public static IInitializationBootstrapper CreateBootstrapper(JToken config)
        {
            if (config["InitializationBootstrapperType"]?.ToString() == "CreateBoostrapperFromWeights")
            {
                var toReturn = config["InitializationBootstrapper"].ToObject<CreateBoostrapperFromWeights>();
                return toReturn;
            }
            return null;
        }
    }
}
