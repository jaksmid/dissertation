using Newtonsoft.Json.Linq;

namespace GeneticProgramming.ComputationNode.Configuration.ExperimentSettings.SubSettings
{
    public interface IPOfNormSettings
    {
        int? P { get; set; }
    }

    class POfNormSettings : IPOfNormSettings
    {
        public int? P { get; set; }

        public POfNormSettings(JToken config)
        {
            var stringP = config.Value<string>("P");
            if (stringP.ToLower() != "max")
            {
                P = config.Value<int>("P");
            }
        }
    }
}