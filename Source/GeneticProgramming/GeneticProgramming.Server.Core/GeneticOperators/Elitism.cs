using GeneticProgramming.Server.Core.GeneticProgramming;
using GeneticProgramming.Server.Core.Helpers;

namespace GeneticProgramming.Server.Core.GeneticOperators
{
    /// <summary>
    /// Elitism operator
    /// </summary>
    public class Elitism:IGeneticOperator
    {
        public BestIndividualSelector BestIndividualSelectorSelector { get; set; }

        public Elitism(BestIndividualSelector bestIndividualSelectorSelector)
        {
            BestIndividualSelectorSelector = bestIndividualSelectorSelector;
        }

        /// <summary>
        /// Modifies population
        /// </summary>
        /// <param name="pop">Population to modify</param>
        public void ModifyPopolation(Population pop)
        {
            var bestInd = BestIndividualSelectorSelector.BestIndivid.CopyIndividual();
            bestInd.NumberOfIndividual = pop.GetNextReservedSlotNumber(pop);
            pop.Individuals[bestInd.NumberOfIndividual] = bestInd;
        }
    }
}