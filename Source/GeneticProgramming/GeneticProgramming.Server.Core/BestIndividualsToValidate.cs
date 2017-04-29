using System.Collections.Generic;
using System.Linq;
using MoreLinq;

namespace GeneticProgramming.Server.Core
{
    public class BestIndividualsToValidate : IValidationSelector
    {
        public List<RateIndividualTask> GetIndividualToValidate(History history)
        {
            var currentGeneration = history.CurrentGeneration;
            var result = currentGeneration.PopulationHistories.Select(pop => new RateIndividualTask(pop.IndividulResults.MaxBy(x => x.Evaluation[0]).EvaluatedIndividual, false, true)).ToList();
            return result;
        }
    }
}