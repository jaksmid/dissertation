using System.Collections.Generic;
using GeneticProgramming.Server.Core.GeneticProgramming;

namespace GeneticProgramming.Server.Core.GeneticOperators.Comparers
{
    public class NsgaComparator : IComparer<Individual>
    {
        public int Compare(Individual x, Individual y)
        {
            int result = CompareDominance(x, y);
            if (result == 0)
            {
                result = CompareCrowding(x, y);
            }
            return result;
        }

        public int CompareCrowding(Individual x, Individual y)
        {
            if (x.CrowdingDistance > y.CrowdingDistance) return 1;
            if (x.CrowdingDistance < y.CrowdingDistance) return -1;
            return 0;
        }

        public int CompareDominance(Individual x, Individual y)
        {
            if (x.Rank < y.Rank) return 1;
            if (x.Rank > y.Rank) return -1;
            return 0;
        }
    }
}