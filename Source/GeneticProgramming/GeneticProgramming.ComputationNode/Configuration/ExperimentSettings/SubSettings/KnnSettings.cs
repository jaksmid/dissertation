using Newtonsoft.Json.Linq;

namespace GeneticProgramming.ComputationNode.Configuration.ExperimentSettings.SubSettings
{
    public class KnnSettings: IKnnSettings
    {
        public int K { get; set; }

        public KnnSettings(JToken config)
        {
            K = config.Value<int>("K");
        }
    }
}
