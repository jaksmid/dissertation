using System;
using System.Collections.Generic;
using GeneticProgramming.Server.Core.GeneticProgramming;
using GeneticProgramming.Server.Core.Helpers;

namespace GeneticProgramming.Server.Core.GeneticOperators.Crossover
{
    /// <summary>
    /// Crossover operator
    /// </summary>
    public class NsgaCrossover:IGeneticOperator
    {
        /// <summary>
        /// Random number generator
        /// </summary>
        Random rnd
        {
            get { return RandomHelpers.rnd; }
        }

        /// <summary>
        /// Crossover probability
        /// </summary>
        private double _crossoverChance = 0.7;

        public double CrossoverChance
        {
            get { return _crossoverChance; }
            set { _crossoverChance = value; }
        }

        /// <summary>
        /// Modifies population
        /// </summary>
        /// <param name="pop">Population to modify</param>
        public void ModifyPopolation(Population pop)
        {
            var tocross = new List<Individual>(2);
            var ind = pop.Individuals;
            var indNumbers = new List<int>();
            var origPopSize = pop.PopulationSize / 2;
            for (int i = origPopSize; i < pop.PopulationSize; i++)
            {
                if (rnd.NextDouble() <= _crossoverChance)
                {
                    indNumbers.Add(i);
                }
            }
            while (indNumbers.Count > 0)
            {
                int index = rnd.Next(indNumbers.Count);
                var currentIndex = indNumbers[index];
                indNumbers.RemoveAt(index);
                Individual indi = ind[currentIndex];
                tocross.Add(indi);
                if (tocross.Count == 2)
                {
                    CrossoverIndividuals(tocross[0], tocross[1]);
                    tocross.Clear();
                }
            }

        }

        /// <summary>
        /// Chance of inner nodes being crossed
        /// </summary>
        private double _crossNodeToLeavesRatio = 0.9;

         private int _programSetNr = 1;
        public int ProgramSetNr
        {
            get { return _programSetNr; }
            set { _programSetNr = value; }
        }

        public NsgaCrossover(int programSetNr)
        {
            _programSetNr = programSetNr;
        }

        public double CrossNodeToLeavesRatio
        {
            get { return _crossNodeToLeavesRatio; }
            set { _crossNodeToLeavesRatio = value; }
        }

        /// <summary>
        /// Crossover two individual
        /// </summary>
        /// <param name="parent1">First individual</param>
        /// <param name="parent2">Second individual</param>
        public void CrossoverIndividuals(Individual parent1, Individual parent2)
        {
            IndividualHelpers.NullEvaluation(parent1);
            IndividualHelpers.NullEvaluation(parent2);
            List<Operator> nodes1;
            List<Operator> nodes2;
            var program1 = parent1.Programs[ProgramSetNr];
            var program2 = parent2.Programs[ProgramSetNr];
            if (rnd.NextDouble() < _crossNodeToLeavesRatio)
            {
                nodes1 = program1.GetInnerNodes;
                if (nodes1.Count == 0) nodes1 = program1.GetLeaves;
            }
            else nodes1 = program1.GetLeaves;

            if (rnd.NextDouble() < _crossNodeToLeavesRatio)
            {
                nodes2 = program2.GetInnerNodes;
                if (nodes2.Count == 0) nodes2 = program2.GetLeaves;
            }
            else nodes2 = program2.GetLeaves;

            //var nodes1 = parent1.GetNodes;
            //var nodes2 = parent2.GetNodes;
            var node1 = nodes1[rnd.Next(nodes1.Count)];
            var node2 = nodes2[rnd.Next(nodes2.Count)];
            var parentnode = node2.Parent;
            if (node1.Parent == null)
            {
                program1.OperatorInstance = node2;
                node2.Parent = null;
            }
            else
            {
                int i = node1.Parent.Children.IndexOf(node1);
                node1.Parent.Children[i] = node2;
                node2.Parent = node1.Parent;
            }
            if (parentnode == null)
            {
                program2.OperatorInstance = node1;
                node1.Parent = null;
            }
            else
            {
                int i = parentnode.Children.IndexOf(node2);
                parentnode.Children[i] = node1;
                node1.Parent = parentnode;
            }
        }
    }
}