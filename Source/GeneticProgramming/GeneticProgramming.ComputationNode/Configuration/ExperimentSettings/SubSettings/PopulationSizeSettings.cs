using Newtonsoft.Json.Linq;

namespace GeneticProgramming.ComputationNode.Configuration.ExperimentSettings.SubSettings
{
    public interface IPopulationSizeSettings
    {
        int PopulationSize { get; set; }
    }

    public class PopulationSizeSettings : IPopulationSizeSettings
    {
        public int PopulationSize { get; set; }

        public PopulationSizeSettings(JToken config)
        {
            PopulationSize = config.Value<int>("PopulationSize");
        }
    }
}
