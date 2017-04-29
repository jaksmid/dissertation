using System.Collections.Generic;

namespace GeneticProgramming.Server.Core
{
    public interface IValidationSelector
    {
        List<RateIndividualTask> GetIndividualToValidate(History history);
    }
}
