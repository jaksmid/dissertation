using System;
using System.Collections.Generic;
using GeneticProgramming.Server.Core.GeneticProgramming;

namespace GeneticProgramming.Server.Core.Nsga
{
    public class CrowdingDistanceCalculator
    {
        public static void CrowdingDistanceAssignment(List<Individual> individuals)
        {
            int size = individuals.Count;

            if (size < 3)
            {
                foreach (var individual in individuals)
                {
                    individual.CrowdingDistance = Double.MaxValue;
                    return;
                }
            }
            var nObjs = individuals[0].MultiObjectiveFitness.Count;
            //Use a new SolutionSet to evite alter original solutionSet
            var front = new List<Individual>();
            for (int i = 0; i < size; i++)
            {
                front.Add(individuals[i]);
            }

            for (int i = 0; i < size; i++)
                front[i].CrowdingDistance = 0.0;

            for (int i = 0; i < nObjs; i++)
            {
                // Sort the population by Obj n
                var comp = new ObjectiveComparator(i);
                front.Sort(comp.Compare);
                //front.sort(new ObjectiveComparator(i));

                double objetiveMinn = front[0].MultiObjectiveFitness[i];
                double objetiveMaxn = front[front.Count - 1].MultiObjectiveFitness[i];

                //Set de crowding distance            
                front[0].CrowdingDistance = Double.MaxValue;
                front[size - 1].CrowdingDistance = Double.MaxValue;

                for (int j = 1; j < size - 1; j++)
                {
                    double distance = front[j + 1].MultiObjectiveFitness[i] - front[j - 1].MultiObjectiveFitness[i];
                    distance = distance / (objetiveMaxn - objetiveMinn);
                    distance += front[j].CrowdingDistance.Value;
                    front[j].CrowdingDistance = distance;
                } // for
            } // for        
        } // crowdingDistanceAssing
    }

    public class ObjectiveComparator : IComparer<Individual>
    {
        private readonly int _objectiveId;

        public ObjectiveComparator(int objective)
        {
            _objectiveId = objective;
        }

        public int Compare(Individual x, Individual y)
        {
            double objectiveX = x.MultiObjectiveFitness[_objectiveId];
            double objectiveY = y.MultiObjectiveFitness[_objectiveId];

            if (objectiveX < objectiveY)
            {
                return -1;
            }
            if (objectiveX > objectiveY)
            {
                return 1;
            }
            return 0;
        }
    }
}