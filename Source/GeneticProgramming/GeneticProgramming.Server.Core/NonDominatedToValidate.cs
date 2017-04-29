using System.Collections.Generic;
using System.Linq;
using GeneticProgramming.Server.Core.Nsga;

namespace GeneticProgramming.Server.Core
{
    public class NonDominatedToValidate : IValidationSelector
    {
        public List<RateIndividualTask> GetIndividualToValidate(History history)
        {
            var currentGeneration = history.CurrentGeneration;
            var currentGenerationIndividuals =
                currentGeneration.PopulationHistories[0].IndividulResults.Select(x => x.EvaluatedIndividual).ToList();
            foreach (var ind in currentGenerationIndividuals)
            {
                ind.Rank = null;
                ind.CrowdingDistance = null;
            }
            DominatedRanking.CreateRanking(currentGenerationIndividuals);
            var result = currentGenerationIndividuals.Where(x => x.Rank == 0).Select(ind => new RateIndividualTask(ind, false, true)).ToList();
            return result;
        }
    }
}