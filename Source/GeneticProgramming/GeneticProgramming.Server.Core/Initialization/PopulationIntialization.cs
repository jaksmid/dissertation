using System.Collections.Generic;
using GeneticProgramming.Server.Core.GeneticProgramming;

namespace GeneticProgramming.Server.Core.Initialization
{
    /// <summary>
    /// Population intialization interface
    /// </summary>
    public interface IPopulationIntialization
    {
        /// <summary>
        /// Initialize individuals
        /// </summary>
        /// <param name="indToCreate">Number of inds to create</param>
        /// <param name="maxDepth">Max depth of individual intialized</param>
        /// <param name="pop"></param>
        /// <param name="initNumber"></param>
        List<Individual> Init(int indToCreate, int maxDepth, Population pop, int initNumber = 0);
    }
}
