using System.Linq;
using GeneticProgramming.Server.Core.GeneticProgramming;
using GeneticProgramming.Server.Core.Helpers;

namespace GeneticProgramming.Server.Core.GeneticOperators.Processing
{
    public class GenerationPostprocess: IGeneticOperator
    {
        public bool FitnessCanChange { get; set; }
        public History ExperimentHistory { get; set; }
        public RateIndividualQueue RatingQueue { get; set; }

        public GenerationPostprocess(bool fitnessCanChange, History experimentHistory, RateIndividualQueue ratingQueue)
        {
            FitnessCanChange = fitnessCanChange;
            ExperimentHistory = experimentHistory;
            RatingQueue = ratingQueue;
        }

        public void ModifyPopolation(Population pop)
        {
            foreach (Individual ind in pop.Individuals)
            {
                ind.GenerationNumber++;
            }
            if (FitnessCanChange)
            {
                pop.Individuals.ForEach(IndividualHelpers.NullEvaluation);
            }
            else
            {
                foreach (Individual ind in pop.Individuals.Where(individual => individual.MultiObjectiveFitness != null))
                {
                    ExperimentHistory.RateIndividual(ind);
                }
            }
            pop.ResetReservedPopulationSlots();
            foreach (Individual ind in pop.Individuals.Where(individual => individual.MultiObjectiveFitness == null))
            {
                RatingQueue.Enqueue(new RateIndividualTask(ind));
            }
        }
    }
}
