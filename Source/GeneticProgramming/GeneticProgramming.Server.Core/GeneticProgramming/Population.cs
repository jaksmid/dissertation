using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeneticProgramming.Server.Core.GeneticOperators;
using GeneticProgramming.Server.Core.Initialization;
using GeneticProgramming.Server.Core.Programs;

namespace GeneticProgramming.Server.Core.GeneticProgramming
{
    /// <summary>
    /// Population
    /// </summary>
    public class Population
    {
        /// <summary>
        /// Genetic operators applied on generation jump
        /// </summary>
        List<IGeneticOperator> _geneticOperators = new List<IGeneticOperator>();

        public List<ProgramTypeSet> TypeSets { get; set; }

        /// <summary>
        /// Genetic operators applied on generation jump
        /// </summary>
        public List<IGeneticOperator> GeneticOperators
        {
            get { return _geneticOperators; }
            set { _geneticOperators = value; }
        }

        public Population()
        {
            
        }

        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="popSize">Population size</param>
        /// <param name="popNumber">Population number</param>
        /// <param name="typeSets"></param>
        /// <param name="initializator"></param>
        public Population(int popSize, int popNumber, List<ProgramTypeSet> typeSets, IPopulationIntialization initializator)
        {
            PopulationNumber = popNumber;
            TypeSets = typeSets;
            PopulationSize = popSize;
            Individuals = initializator.Init(PopulationSize, 6, this);           
        }

        /// <summary>
        /// Population number
        /// </summary>
        public int PopulationNumber { get; set; }

        /// <summary>
        /// All individuals in population
        /// </summary>
        public List<Individual> Individuals { get; set; }

        public List<Individual> NonReservedIndividuals
        {
            get { return Individuals.Take(FirstReservedSlot).ToList(); }
        }

        /// <summary>
        /// Generation number
        /// </summary>
        public int GenerationNumber
        {
            get { return Individuals[0].GenerationNumber; }
        }

        /// <summary>
        /// Next generation jump
        /// </summary>
        public void NextGeneration()
        {
            foreach (var v in _geneticOperators)
            {
              v.ModifyPopolation(this);
            }
        }

        /// <summary>
        /// Get individual
        /// </summary>
        /// <param name="numberOfIndividual">Identification of individual to retrieve</param>
        /// <returns>Individual</returns>
        public Individual GetIndividual(int numberOfIndividual)
        {
            return Individuals[numberOfIndividual];
        }

        /// <summary>
        /// Population size
        /// </summary>
        public int PopulationSize
        {
            get; set;
        }

        protected int ReservedSlots = 0;

        public void ReservePopulationSlots(int numberOfIndividuals)
        {
            ReservedSlots += numberOfIndividuals;
        }

        public void ResetReservedPopulationSlots()
        {
            ReservedSlots = 0;
        }

        public int GetNextReservedSlotNumber(Population population)
        {
            var toReturn = FirstReservedSlot;
            ReservedSlots--;
            return toReturn;
        }

        public int FirstReservedSlot { get { return PopulationSize - ReservedSlots; } }

        public void LogStatus(StringBuilder sb, bool includeValidation=true)
        {
            //sb.Append("Best results:");
            //int objectives = Individuals.First().MultiObjectiveFitness.Count;
            //for (int k = 0; k < objectives; k++)
            //{
            //    if (k > 0) sb.Append(" ");
            //    sb.Append("Objective " + k + ": ");
            //    var inds = Individuals.Where(x => x.MultiObjectiveFitness != null);
            //    var bestIndivual = inds.Max(x => x.MultiObjectiveFitness[k]);
            //    sb.Append(bestIndivual);
            //}
            //sb.Append(Environment.NewLine);
            //sb.Append(" First front:");
            //var best = Individuals.Where(ind => ind.Rank == 0);
            //var individuals = best as IList<Individual> ?? best.ToList();
            //int items = 0;
            //for (int k = 0; k < objectives; k++)
            //{
            //    int k1 = k;
            //    var toAppend = string.Join(";", individuals.Select(x => x.MultiObjectiveFitness[k1]));
            //    int items2 = toAppend.Count(l => l == ';');
            //    if (items != 0 && items != items2)
            //    {
            //        string a = "problem occurred";
            //    }
            //    items = items2;
            //    sb.Append(toAppend);
            //    if (k1 != objectives - 1)
            //    {
            //        sb.Append("%");
            //    }
            //}
            //sb.Append(Environment.NewLine);
            //if (includeValidation)
            //{
            //    sb.Append(" Validation: ");
            //    for (int k = 0; k < objectives; k++)
            //    {
            //        int k1 = k;
            //        var indsWithVal = individuals.Where(ka => ka.MultiObjectiveValidation != null);
            //        sb.Append(string.Join(";", indsWithVal.Select(x => x.MultiObjectiveValidation[k1])));
            //        if (k1 != objectives - 1)
            //        {
            //            sb.Append("%");
            //        }
            //    }
            //}
            //sb.Append("%");
        }
    }
}