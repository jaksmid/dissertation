using GeneticProgramming.Server.Core.GeneticProgramming;
using MoreLinq;

namespace GeneticProgramming.Server.Core.GeneticOperators.Validation
{
    public class ValidateBest:IGeneticOperator
    {
        public RateIndividualQueue RatingQueue { get; set; }

        public ValidateBest(RateIndividualQueue ratingQueue)
        {
            RatingQueue = ratingQueue;
        }


        public void ModifyPopolation(Population pop)
        {
            var best = pop.Individuals.MaxBy(x => x.MultiObjectiveFitness[0]);
            RatingQueue.Enqueue(new RateIndividualTask(best, false, true));
        }
    }
}
