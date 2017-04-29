using GeneticProgramming.Server.Core.GeneticProgramming;

namespace GeneticProgramming.Server.Core.EndCriterions
{
    public interface IEndCriterion
    {
        bool IsEvaluationFinished(Populations populations,int generationNumber);
    }

    public class GenerationEndCriterion : IEndCriterion
    {
        private readonly int _generationCount;

        public GenerationEndCriterion(int generationCount)
        {
            _generationCount = generationCount;
        }

        public bool IsEvaluationFinished(Populations populations, int generationNumber)
        {
            //var experimentControl = _provider.GetExperimentControl(experimentName);
            if (_generationCount <= generationNumber)
            {
                return true;
            }
            return false;
        }
    }
}