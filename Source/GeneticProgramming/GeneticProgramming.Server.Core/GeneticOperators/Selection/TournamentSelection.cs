using System.Collections.Generic;
using GeneticProgramming.Server.Core.GeneticProgramming;

namespace GeneticProgramming.Server.Core.GeneticOperators.Selection
{
    /// <summary>
    /// Tournamnet selection
    /// </summary>
    public class TournamentSelection:BaseTournamentSelection
    {
        public TournamentSelection(double probabilityOfBetterWinning, int tournamentSize, IComparer<Individual> comparer = null) :
            base(probabilityOfBetterWinning, tournamentSize, comparer)
        {
        }

        /// <summary>
        /// Modifies population
        /// </summary>
        /// <param name="pop">Population to modify</param>
        public override void ModifyPopolation(Population pop)
        {
            var subPopulation = pop.Individuals;
            var toAdd = new List<Individual>();
            for (int i = 0; i < pop.FirstReservedSlot; i++){
                var indi = GetCopyOfRandomIndividual(subPopulation);
                toAdd.Add(indi);
            }
            for (int i = 0; i < pop.FirstReservedSlot; i++)
            {
                var indi = toAdd[i];
                indi.NumberOfIndividual = i;
                pop.Individuals[i] = indi;
            }
        }
    }
}