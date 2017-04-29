using Newtonsoft.Json.Linq;

namespace GeneticProgramming.ComputationNode.Configuration.ExperimentSettings.SubSettings
{
    public class CoevolutionSettings : ICoevolutionSettings
    {
        public CoevolutionSettings(JToken config)
        {
            UseCoevolution = config.Value<bool>("Coevolution");
        }

        public bool UseCoevolution { get; set; }
    }
}