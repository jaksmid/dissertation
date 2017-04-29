using System;
using System.Collections.Generic;
using GeneticProgramming.Server.Core.GeneticProgramming;
using GeneticProgramming.Server.Core.Helpers;

namespace GeneticProgramming.Server.Core.GeneticOperators
{
    /// <summary>
    /// Selection roulette
    /// </summary>
    public class SelectionRoulette:IGeneticOperator
    {
        /// <summary>
        /// Random numbers generator
        /// </summary>
        Random rnd
        {
            get { return RandomHelpers.rnd; }
        }
        /// <summary>
        /// sum of population fitness
        /// </summary>
        double _fitnesSum;

        /// <summary>
        /// Modifies population
        /// </summary>
        /// <param name="pop">Population to modify</param>
        public void ModifyPopolation(Population pop)
        {
            _fitnesSum = 0;
            foreach (var ind in pop.Individuals)
            {
                _fitnesSum += (double)ind.Fitness;
            }
            var individ = new List<Individual>();
            for (int i = 0; i < pop.FirstReservedSlot; i++)
            {
                var indi = SelectRandomIndividual(pop.Individuals);
                individ.Add(indi);
                indi.NumberOfIndividual = i;
            }
            pop.Individuals = individ;
        }

        /// <summary>
        /// One spin of selection roulette
        /// </summary>
        /// <param name="individuals">List of individuals to choose from</param>
        /// <returns></returns>
        public Individual SelectRandomIndividual(List<Individual> individuals)
        {
            double tempf = 0;
            double newd = rnd.NextDouble() * _fitnesSum;
            foreach (var ind in individuals)
            {
                if (tempf + (double)ind.Fitness >= newd) return ind.CopyIndividual();
                tempf += (double)ind.Fitness;
            }
            return null;

        }
    }
}