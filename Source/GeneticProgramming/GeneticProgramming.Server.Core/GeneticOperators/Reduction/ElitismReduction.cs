using System.Collections.Generic;
using System.Linq;
using GeneticProgramming.Server.Core.GeneticProgramming;

namespace GeneticProgramming.Server.Core.GeneticOperators.Reduction
{
    /// <summary>
    /// Mutation operator
    /// </summary>
    public class ElitismReduction:IGeneticOperator
    {
        private readonly IComparer<Individual> _comparer;
        private bool _first = true;

        public ElitismReduction(IComparer<Individual> comparer)
        {
            _comparer = comparer;
        }

        /// <summary>
        /// Modifies population
        /// </summary>
        /// <param name="pop">Population to modify</param>
        public void ModifyPopolation(Population pop)
        {
            
            if (_first)
            {
                _first = false;
                return;
            }
            pop.PopulationSize = pop.PopulationSize/2;
            var newPopulation=pop.Individuals.OrderByDescending(x => x, _comparer).Take(pop.PopulationSize).ToList();
            pop.Individuals = newPopulation;
            for (int i = 0; i < pop.PopulationSize; i++)
            {
                pop.Individuals[i].NumberOfIndividual = i;
            }
        }
    }
}