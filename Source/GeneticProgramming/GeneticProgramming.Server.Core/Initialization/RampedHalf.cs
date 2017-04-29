using System.Collections.Generic;
using GeneticProgramming.Server.Core.GeneticProgramming;
using GeneticProgramming.Server.Core.Programs;

namespace GeneticProgramming.Server.Core.Initialization
{
    /// <summary>
    /// Ramped half initialization
    /// </summary>
    public class RampedHalf:IPopulationIntialization
    {
        public RampedHalf(Grow grow, Full full)
        {
             _grow = grow;
             _full =  full;
        }

        public RampedHalf(List<ProgramTypeSet> typeset)
        {
            _grow = new Grow(typeset);
            _full = new Full(typeset);
        }

        /// <summary>
        /// Grow intializator
        /// </summary>
        private readonly Grow _grow;

        /// <summary>
        /// Full intializator
        /// </summary>
        private readonly Full _full;

        /// <summary>
        /// Initialize individuals
        /// </summary>
        /// <param name="indToCreate">Number of inds to create</param>
        /// <param name="maxDepth">Max depth of individual intialized</param>
        /// <param name="pop"></param>
        /// <param name="initNumber"></param>
        public List<Individual> Init(int indToCreate, int maxDepth, Population pop, int initNumber = 0)
        {
            var toReturn =new List<Individual>();
            var split = indToCreate/2;
            for (int i = 0; i < indToCreate; i++)
            {
                var ind = new Individual
                {
                    NumberOfIndividual = i + initNumber,
                    PopulationNumber = pop.PopulationNumber
                };
                if (i < split)
                {
                    ind.Programs = _full.CreatePrograms(maxDepth);
                }
                else ind.Programs = _grow.CreatePrograms(maxDepth);
                toReturn.Add(ind);
            }
            return toReturn;
        }
    }
}