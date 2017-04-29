using Newtonsoft.Json;

namespace GeneticProgramming.Data.Reporting
{
    public class DetailedGenerationProgress : IGenerationProgress
    {
        public double MaxFitness { get; set; }

        public double? MaxEvaluation { get; set; }

        public double MinFitness { get; set; }

        public double AverageFitness { get; set; }

        public double[] AverageNumberOfNodes { get; set; }

        public string ToJsonString()
        {
           string json = JsonConvert.SerializeObject(this);
            return json;
        }
    }
}