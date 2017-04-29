using System;
using System.Collections.Generic;
using GeneticProgramming.Server.Core.GeneticProgramming;
using GeneticProgramming.Server.Core.Helpers;

namespace GeneticProgramming.Server.Core.GeneticOperators.Crossover
{
    /// <summary>
    /// Crossover operator
    /// </summary>
    public class Crossover:IGeneticOperator
    {
        private readonly int _crossoverProgram;

        /// <summary>
        /// Random number generator
        /// </summary>
        Random rnd
        {
            get { return RandomHelpers.rnd; }
        }

        public double CrossoverChance { get; set; }

        /// <summary>
        /// Modifies population
        /// </summary>
        /// <param name="pop">Population to modify</param>
        public void ModifyPopolation(Population pop)
        {
            var tocross = new List<Individual>();
            var ind = new List<Individual>();
            ind.AddRange(pop.NonReservedIndividuals);
            while (ind.Count>0)
            {
                int index = rnd.Next(ind.Count);
                Individual indi = ind[index];
                ind.RemoveAt(index);
                if (rnd.NextDouble() <= CrossoverChance)
                {
                    tocross.Add(indi);
                    if (tocross.Count == 2)
                    {
                        CrossoverIndividuals(tocross[0], tocross[1]);
                        tocross = new List<Individual>();
                    }
                }
            }
        }

        /// <summary>
        /// Chance of inner nodes being crossed
        /// </summary>
        private double _crossNodeToLeavesRatio = 0.9;

        public Crossover(double crossoverChance = 0.7, int crossoverProgram=0)
        {
            CrossoverChance = crossoverChance;
            _crossoverProgram = crossoverProgram;
        }

        public double CrossNodeToLeavesRatio
        {
            get { return _crossNodeToLeavesRatio; }
            set { _crossNodeToLeavesRatio = value; }
        }

        /// <summary>
        /// Crossover two individual
        /// </summary>
        /// <param name="parentLeft">First individual</param>
        /// <param name="parentRight">Second individual</param>
        public void CrossoverIndividuals(Individual parentLeft, Individual parentRight)
        {
            IndividualHelpers.NullEvaluation(parentLeft);
            IndividualHelpers.NullEvaluation(parentRight);
            var parent1 = parentLeft.Programs[_crossoverProgram];
            var parent2 = parentRight.Programs[_crossoverProgram];
            List<Operator> nodes1;
            List<Operator> nodes2;
            if (rnd.NextDouble() < _crossNodeToLeavesRatio)
            {
                nodes1 = parent1.GetInnerNodes;
                if (nodes1.Count == 0) nodes1 = parent1.GetLeaves;
            }
            else nodes1 = parent1.GetLeaves;

            if (rnd.NextDouble() < _crossNodeToLeavesRatio)
            {
                nodes2 = parent2.GetInnerNodes;
                if (nodes2.Count == 0) nodes2 = parent2.GetLeaves;
            }
            else nodes2 = parent2.GetLeaves;

            //var nodes1 = parent1.GetNodes;
            //var nodes2 = parent2.GetNodes;
            var node1 = nodes1[rnd.Next(nodes1.Count)];
            var node2 = nodes2[rnd.Next(nodes2.Count)];
            var parentnode = node2.Parent;
            if (node1.Parent == null)
            {
                parent1.OperatorInstance = node2;
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
                parent2.OperatorInstance = node1;
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