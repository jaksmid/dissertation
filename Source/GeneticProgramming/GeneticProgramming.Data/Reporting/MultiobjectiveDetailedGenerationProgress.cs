using System.Collections.Generic;
using Newtonsoft.Json;

namespace GeneticProgramming.Data.Reporting
{
    public class MultiobjectiveDetailedGenerationProgress : IGenerationProgress
    {
        public double[] MaxObjectives { get; set; }

        public double[] MaxObjectivesValidation { get; set; }

        public List<List<double>> FirstFrontFitness { get; set; }

        public List<List<double>> FirstFrontValidation { get; set; }

        public double[] AverageNumberOfNodes { get; set; }

        public string ToJsonString()
        {
            string json = JsonConvert.SerializeObject(this);
            return json;
        }
    }
}