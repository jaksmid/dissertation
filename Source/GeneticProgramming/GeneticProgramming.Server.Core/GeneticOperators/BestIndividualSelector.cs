using GeneticProgramming.Server.Core.GeneticProgramming;
using GeneticProgramming.Server.Core.Helpers;
using MoreLinq;

namespace GeneticProgramming.Server.Core.GeneticOperators
{
    public class BestIndividualSelector :IGeneticOperator
    {
        /// <summary>
        /// Best individual
        /// </summary>
        public Individual BestIndivid { get; set; }

        public void ModifyPopolation(Population pop)
        {
            var bestIndivid = pop.Individuals.MaxBy(ind => ind.Fitness).CopyIndividual();
            pop.ReservePopulationSlots(1);
            BestIndivid = bestIndivid;
        }
    }
}