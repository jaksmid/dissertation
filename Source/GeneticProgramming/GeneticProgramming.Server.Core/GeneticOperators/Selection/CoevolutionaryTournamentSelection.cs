using System.Collections.Generic;
using GeneticProgramming.Server.Core.GeneticProgramming;

namespace GeneticProgramming.Server.Core.GeneticOperators.Selection
{
    /// <summary>
    /// Tournamnet selection
    /// </summary>
    public class CoevolutionaryTournamentSelection:BaseTournamentSelection
    {
        public CoevolutionaryTournamentSelection(double probabilityOfBetterWinning, int tournamentSize, IComparer<Individual> comparer = null) :
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
            for (int i = 0; i < pop.FirstReservedSlot; i++){
                var indi = new Individual
                {
                    Programs = new List<GpProgram>()
                };
                for (int k = 0; k < pop.TypeSets.Count; k++)
                {
                    var programK = GetCopyOfRandomIndividual(subPopulation).Programs[k];
                    indi.Programs.Add(programK);
                }
                indi.NumberOfIndividual = i;
                pop.Individuals[i] = indi;
            }
        }
    }
}