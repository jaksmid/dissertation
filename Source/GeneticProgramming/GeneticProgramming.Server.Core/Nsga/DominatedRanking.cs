using System.Collections.Generic;
using GeneticProgramming.Server.Core.GeneticProgramming;

namespace GeneticProgramming.Server.Core.Nsga
{
    public class DominatedRanking
    {
        // The solution set to be ranked

        // An array containing all the fronts found during the search

        // Comparator for dominance checking
        private static readonly IComparer<Individual> Dominance = new DominanceComparator();

        public static void CreateRanking(List<Individual> individuals)
        {
            List<Individual> individuals1 = individuals;

            // dominateMe[i] contains the number of solutions dominating i
            int[] dominateMe = new int[individuals1.Count];

            // iDominate[k] contains the list of solutions dominated by k
            var iDominate = new List<int>[individuals1.Count];

            // front[i] contains the list of individuals belonging to the front i
            //List<int>[] front = new List<int>[_individuals.size () + 1];

            var front2 = new List<List<int>>();

            // flagDominate is an auxiliar variable
            int flagDominate;

            // Initialize the fronts
            //for (int i = 0; i < front.Length; i++)
            //front[i] = new List<int> ();

            front2.Add(new List<int>());

            // Fast non dominated sorting algorithm
            for (int p = 0; p < individuals1.Count; p++)
            {
                // Initialice the list of individuals that i dominate and the number
                // of individuals that dominate me
                iDominate[p] = new List<int>();
                dominateMe[p] = 0;
                // For all q individuals , calculate if p dominates q or vice versa
                for (int q = 0; q < individuals1.Count; q++)
                {
                    //flagDominate =constraint_.compare(individuals.get(p),individuals.get(q));
                    //if (flagDominate == 0) {
                    flagDominate = Dominance.Compare(individuals[p], individuals[q]);
                    //}

                    if (flagDominate == 1)
                    {
                        iDominate[p].Add(q);
                    }
                    else if (flagDominate == -1)
                    {
                        dominateMe[p]++;
                    }
                }

                // If nobody dominates p, p belongs to the first front
                if (dominateMe[p] == 0)
                {
                    //front[0].Add (p);
                    front2[0].Add(p);
                    individuals[p].Rank = 0;
                }
            }

            //Obtain the rest of fronts
            int currentRank = 0;
            //Iterator<Integer> it1, it2 ; // Iterators
            //while (front[currentRank].Capacity != 0) {

            while (front2[currentRank].Count != 0)
            {
                currentRank++;
                front2.Add(new List<int>());
                //front[currentRank].Clear ();
                //foreach (int p in front[currentRank - 1]) {
                foreach (int p in front2[currentRank - 1])
                {
                    foreach (int q in iDominate[p])
                    {
                        dominateMe[q]--;
                        if (dominateMe[q] == 0)
                        {
                            individuals1[q].Rank = currentRank;
                            //  front[currentRank].Add (q);
                            front2[currentRank].Add(q);
                        }
                    }
                }
            }

            //var ranking = new List<Individual>[currentRank];
            //for (int i = 0; i < currentRank; i++)
            //{
            //    //ranking_[i] = new SolutionSet (front[i].Capacity);
            //    ranking[i] = new List<Individual>(front2[i].Count);
            //    //foreach (int elem in front[i])
            //    foreach (int elem in front2[i])
            //    {
            //        ranking[i].Add(individuals1[elem]);
            //    }
            //    // foreach
            //}
            //// for

        }
    }

    public class DominanceComparator : IComparer<Individual>
    {
        public int Compare(Individual x, Individual y)
        {
            bool dominate1 =false; // dominate1 indicates if some objective of x
            // dominates the same objective in y.
            bool dominate2 =false; // Complementary of dominate1

            if (x == null)
                return 1;
            if (y == null)
                return -1;

            var solution1 = x;
            var solution2 = y;

            // Dominance Test
            for (int i = 0; i < solution1.MultiObjectiveFitness.Count; i++)
            {
                double value1 = solution1.MultiObjectiveFitness[i];
                double value2 = solution2.MultiObjectiveFitness[i];
                if (value1 > value2)
                {
                    dominate1 = true;
                }
                else if (value1 < value2)
                {
                    dominate2 = true;
                }
            }

            if (dominate1 == dominate2)
            {
                return 0; //No one dominate the other
            }
            if (dominate1)
            {
                return 1; // solution1 dominate
            }
            return -1;    // solution2 dominate
        }
    }
}