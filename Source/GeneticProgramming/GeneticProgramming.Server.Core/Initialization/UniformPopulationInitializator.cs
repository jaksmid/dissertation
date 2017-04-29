using System.Collections.Generic;
using GeneticProgramming.Server.Core.GeneticProgramming;

namespace GeneticProgramming.Server.Core.Initialization
{
    public class UniformPopulationInitializator: IPopulationIntialization
    {
        public IProgramInitializator Initializator { get; set; }

        public UniformPopulationInitializator(IProgramInitializator initializator)
        {
            Initializator = initializator;
        }

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
            for (int i = 0; i < indToCreate; i++)
            {
                var ind = new Individual
                {
                    Programs = Initializator.CreatePrograms(maxDepth),
                    NumberOfIndividual = i + initNumber,
                    PopulationNumber = pop.PopulationNumber
                };
                toReturn.Add(ind);
            }
            return toReturn;
        }
    }
}
