using System.Collections.Generic;
using System.Linq;
using GeneticProgramming.Server.Core.GeneticProgramming;
using GeneticProgramming.Server.Core.Helpers;

namespace GeneticProgramming.Server.Core.GeneticOperators.Selection
{
    /// <summary>
    /// Tournamnet selection
    /// </summary>
    public class NonDestructiveTournamentSelection:BaseTournamentSelection
    {
        public NonDestructiveTournamentSelection(double probabilityOfBetterWinning, int tournamentSize, IComparer<Individual> comparer = null) : 
            base(probabilityOfBetterWinning, tournamentSize, comparer)
        {
        }

        /// <summary>
        /// Modifies population
        /// </summary>
        /// <param name="pop">Population to modify</param>
        public override void ModifyPopolation(Population pop)
        {
            var oldIndividualsCopy = pop.Individuals.Select(x => x.CopyIndividual(true)).ToList();
            var newIndividuals = new List<Individual>();
            var popSize=pop.PopulationSize;
            int initialSlot = popSize;
            pop.PopulationSize += popSize;
            for (int i = 0; i < popSize; i++){
                var indi = GetCopyOfRandomIndividual(pop.Individuals);
                var indNr = i + initialSlot;
                indi.NumberOfIndividual = indNr;
                newIndividuals.Add(indi);
            }
            pop.Individuals = oldIndividualsCopy.Concat(newIndividuals).ToList();
        }
    }
}
