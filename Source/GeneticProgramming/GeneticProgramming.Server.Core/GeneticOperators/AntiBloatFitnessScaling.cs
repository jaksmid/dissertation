using System;
using System.Linq;
using GeneticProgramming.Server.Core.GeneticProgramming;

namespace GeneticProgramming.Server.Core.GeneticOperators
{
    /// <summary>
    /// Operator that scales fitness of BLOATed individuals
    /// </summary>
    public class AntiBloatFitnessScaling:IGeneticOperator
    {
        /// <summary>
        /// Modifies population
        /// </summary>
        /// <param name="pop">Population to modify</param>
        public void ModifyPopolation(Population pop)
        {
            foreach (var ind in pop.Individuals)
            {
                var v = ind.Programs.First();
                double newfitness=ind.Fitness.Value;
                var statistic = v.Statistic;
                if (statistic.Depth > 10)
                {
                    newfitness = newfitness - 100 * (statistic.Depth.Value - 10);
                }
                if (statistic.Width > 10)
                {
                    newfitness = Math.Min(ind.Fitness.Value - 150 * (statistic.Width.Value - 10), newfitness);
                }
                if (statistic.NumberOfNodes > 80)
                {
                    newfitness = Math.Min(ind.Fitness.Value - 10 * (statistic.NumberOfNodes.Value / 5), newfitness);
                }
                ind.MultiObjectiveFitness[0] = newfitness;
            }
        }
    }
}