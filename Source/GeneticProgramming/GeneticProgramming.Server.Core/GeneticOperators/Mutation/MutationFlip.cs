using System;
using System.Collections.Generic;
using GeneticProgramming.Server.Core.GeneticProgramming;
using GeneticProgramming.Server.Core.Helpers;

namespace GeneticProgramming.Server.Core.GeneticOperators.Mutation
{
    /// <summary>
    /// Flip mutation
    /// </summary>
    public class MutationFlip:IGeneticOperator
    {
        /// <summary>
        /// Random number generator
        /// </summary>
        Random rnd
        {
            get { return RandomHelpers.rnd; }
        }

        /// <summary>
        /// Modifies population
        /// </summary>
        /// <param name="pop">Population to modify</param>
        public void ModifyPopolation(Population pop)
        {
            foreach (var ind in pop.Individuals)
            {
                if (rnd.NextDouble() < ChanceOfMutation)
                {
                    //var nodes=singleInd.GetNodes;
                    //var op = nodes[rnd.Next(nodes.Count)];

                    throw new NotImplementedException();
                }
            }
        }

        /// <summary>
        /// Mutation chance
        /// </summary>
        public double ChanceOfMutation = 0.01;

        /// <summary>
        /// Mutation is computed per NODE/per Tree not implemented
        /// </summary>
        public bool EvaluateAllNodes = false;

        /// <summary>
        /// Intialization
        /// </summary>
        /// <param name="chanceOfMutation">Mutation chance</param>
        public MutationFlip(double chanceOfMutation)
        {
            ChanceOfMutation = chanceOfMutation;
        }

        /// <summary>
        /// Gets execution orders
        /// </summary>
        /// <returns>List of execution orders</returns>
        public List<int> GetExecutionOrders()
        {
            return new List<int> { 1 };
        }
    }
}