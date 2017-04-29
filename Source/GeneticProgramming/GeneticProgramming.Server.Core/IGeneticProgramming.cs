using GeneticProgramming.Data.Contracts;

namespace GeneticProgramming.Server.Core
{
    public interface IGeneticProgramming
    {
        ProgramEnvelope GetIndividual();
        void RateIndividual(IndividualEvaluationResults result);
        ProgramEnvelope GetSpecificIndividual(int generation, int populationNumber, int individualNumber);
    }
}
