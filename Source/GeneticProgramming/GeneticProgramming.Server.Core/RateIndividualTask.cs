using GeneticProgramming.Server.Core.GeneticProgramming;
using GeneticProgramming.Server.Core.Helpers;

namespace GeneticProgramming.Server.Core
{
    public class RateIndividualTask
    {
        public static int LatestId = 0;

        public bool Evaluate { get; set; }
        public bool Validate { get; set; }

        public bool ComputeStatistics { get { return Evaluate; }}

        public int Id { get; set; }

        public Individual IndividualToRate { get; set; }

        public RateIndividualTask(Individual individual, bool evaluate = true, bool validate = false)
        {
            Id = LatestId;
            LatestId ++;
            IndividualToRate = individual;
            Evaluate = evaluate;
            Validate = validate;
        }
    }
}
