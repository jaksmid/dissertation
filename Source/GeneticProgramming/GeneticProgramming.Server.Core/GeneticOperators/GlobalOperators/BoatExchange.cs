using System.Collections.Generic;
using GeneticProgramming.Server.Core.GeneticProgramming;

namespace GeneticProgramming.Server.Core.GeneticOperators.GlobalOperators
{
    /// <summary>
    /// Exchange best individuals between generations
    /// </summary>
    public class BoatExchange : IGlobalGeneticOperator
    {
        /// <summary>
        /// How often should this operator be executed
        /// </summary>
        public int Periodicity = 10;

        /// <summary>
        /// To each population list of new individuals
        /// </summary>
        public Dictionary<Population, SortedList<double, Individual>> individualsToAdd = new Dictionary<Population, SortedList<double, Individual>>();

        /// <summary>
        /// Modifies populations
        /// </summary>
        /// <param name="populations">Populations to modify</param>
        public void ModifyPopolation(Populations populations)
        {
            //if (populations.Islands.Count < 2 || populations.GenerationNumber == 0) return;
            //if (populations.GenerationNumber % Periodicity == 0)
            //{
            //    if (_selectionPhase)
            //    {
            //        _selectionPhase = false;
            //        individualsToAdd.Clear();
            //        foreach (var pop in populations.Islands)
            //        {
            //            individualsToAdd.Add(pop, new SortedList<double, Individual>());
            //            foreach (var pop2 in populations.Islands)
            //            {
            //                if (pop != pop2)
            //                {
            //                    if (!individualsToAdd[pop].ContainsKey(pop2.BestIndividual.Fitness.Value)) individualsToAdd[pop].Add(pop2.BestIndividual.Fitness.Value, IndividualHelpers.CopyIndividual(pop2.BestIndividual));
            //                }
            //            }
            //            pop.ReservedPlaces += individualsToAdd[pop].Count;
            //        }
            //    }
            //    else
            //    {
            //        _selectionPhase = true;
            //        foreach (var v in individualsToAdd)
            //        {
            //            foreach (var ind in v.Value.Values)
            //            {
            //                ind.NumberOfIndividual = populations.Islands[v.Key.PopulationNumber].Individuals.Count;
            //                ind.PopulationNumber = v.Key.PopulationNumber;
            //                populations.Islands[v.Key.PopulationNumber].Individuals.Add(ind);
            //            }
            //        }
            //    }
            //}
        }
    }
}