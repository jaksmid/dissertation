using System.Collections.Generic;
using GeneticProgramming.Server.Core.GeneticProgramming;

namespace GeneticProgramming.Server.Core.GeneticOperators.Comparers
{
    public class FitnessComparer:IComparer<Individual>
    {
        public int Compare(Individual x, Individual y)
        {
            if (x.Fitness == null || y.Fitness == null) return 0;
            return x.Fitness.Value.CompareTo(y.Fitness.Value);
        }
    }
}