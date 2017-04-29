using System;
using GeneticProgramming.Server.Core.GeneticProgramming;
using GeneticProgramming.Server.Core.Helpers;
using GeneticProgramming.Server.Core.Initialization;

namespace GeneticProgramming.Server.Core.GeneticOperators.Mutation
{
    /// <summary>
    /// Mutation operator
    /// </summary>
    public class Mutation:IGeneticOperator
    {
        /// <summary>
        /// Random number generator
        /// </summary>
        Random rnd
        {
            get { return RandomHelpers.rnd; }
        }

        public double MutationChance { get; set; }

        public int ProgramSetNr { get; set; }

        /// <summary>
        /// Depths of new grown individual
        /// </summary>
        public int Depthmutation = 5;

        /// <summary>
        /// Grow intialization of new individuals
        /// </summary>
        IProgramInitializator IndCreator { get; set; }

        public Mutation(double mutationChance, IProgramInitializator indCreator, int programSetNr = 0)
        {
            MutationChance = mutationChance;
            ProgramSetNr = programSetNr;
            IndCreator = indCreator;
        }

        /// <summary>
        /// Modifies population
        /// </summary>
        /// <param name="pop">Population to modify</param>
        public void ModifyPopolation(Population pop)
        {
            var ind = pop.NonReservedIndividuals;
            foreach (var individual in ind)
            {
                if (rnd.NextDouble() <= MutationChance)
                {
                    MutateIndividual(individual,pop);
                }
            }
        }

        /// <summary>
        /// Mutates individual
        /// </summary>
        /// <param name="individual">Individual to mutate</param>
        /// <param name="pop">Origin population</param>
        public void MutateIndividual(Individual individual,Population pop)
        {
            IndividualHelpers.NullEvaluation(individual);
            var indSet = individual.Programs[ProgramSetNr];
            var node1 = indSet.GetNodes[rnd.Next(indSet.GetNodes.Count)];
            var opers = IndCreator.CreatePrograms(Depthmutation);
            if (node1.Parent == null)
            {
                indSet.OperatorInstance = opers[ProgramSetNr].OperatorInstance;
            }
            else
            {
                var oper=opers[ProgramSetNr].OperatorInstance;
                oper.Parent = node1.Parent;
                int index = oper.Parent.Children.IndexOf(node1);
                oper.Parent.Children[index] = oper;
            }

        }
    }
}