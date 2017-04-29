using System;
using System.Collections.Generic;
using System.Linq;
using GeneticProgramming.Server.Core.GeneticProgramming;
using GeneticProgramming.Server.Core.Helpers;
using GeneticProgramming.Server.Core.Initialization;

namespace GeneticProgramming.Server.Core.GeneticOperators.Mutation
{
    /// <summary>
    /// Mutation operator
    /// </summary>
    public class NsgaMutation:IGeneticOperator
    {
        public IProgramInitializator IndCreator { get; set; }

        public NsgaMutation(IProgramInitializator indCreator, int programSetNr)
        {
            IndCreator = indCreator;
            _programSetNr = programSetNr;
        }

        /// <summary>
        /// Random number generator
        /// </summary>
        Random rnd
        {
            get { return RandomHelpers.rnd; }
        }

        /// <summary>
        /// Chance of individual to bem utated
        /// </summary>
        private double _mutationChance = 0.2;

        private int _programSetNr = 1;

        public double MutationChance
        {
            get { return _mutationChance; }
            set { _mutationChance = value; }
        }

        public int ProgramSetNr
        {
            get { return _programSetNr; }
            set { _programSetNr = value; }
        }

        /// <summary>
        /// Depths of new grown individual
        /// </summary>
        public int Depthmutation = 5;

        /// <summary>
        /// Modifies population
        /// </summary>
        /// <param name="pop">Population to modify</param>
        public void ModifyPopolation(Population pop)
        {
            var origPopSize = pop.PopulationSize/2;
            var ind = pop.Individuals.Where(i => i.NumberOfIndividual >= origPopSize);
            var toMutate = new List<int>();
            foreach (var individual in ind)
            {
                var diceresult = rnd.NextDouble();
                if (diceresult <= MutationChance)
                {
                    toMutate.Add(individual.NumberOfIndividual);
                }
            }
            foreach (var individualNumber in toMutate)
            {
                    MutateIndividual(pop.Individuals[individualNumber],pop);
                
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
            var opers=IndCreator.CreatePrograms(Depthmutation);
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