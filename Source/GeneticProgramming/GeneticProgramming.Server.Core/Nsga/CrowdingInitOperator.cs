using GeneticProgramming.Server.Core.GeneticOperators;
using GeneticProgramming.Server.Core.GeneticProgramming;

namespace GeneticProgramming.Server.Core.Nsga
{
    public class CrowdingInitOperator:IGeneticOperator
    {
        public void ModifyPopolation(Population pop)
        {
            foreach (var ind in pop.Individuals)
            {
                ind.Rank = null;
                ind.CrowdingDistance = null;
            }
            DominatedRanking.CreateRanking(pop.Individuals);
            CrowdingDistanceCalculator.CrowdingDistanceAssignment(pop.Individuals);
        }
    }
}