using System.Collections.Generic;
using System.Linq;
using GeneticProgramming.Data.Contracts;
using GeneticProgramming.Data.Statistics;

namespace GeneticProgramming.Server.Core.GeneticProgramming
{
    /// <summary>
    /// Individual
    /// </summary>
    public class Individual
    {
        public int GenerationNumber { get; set; }

        /// <summary>
        /// Number of population in wchid individual resides
        /// </summary>
        public int PopulationNumber { get; set; }

        /// <summary>
        /// Identification number
        /// </summary>
        public int NumberOfIndividual { get; set; }

        /// <summary>
        /// Fitness
        /// </summary>
        public double? Fitness
        {
            get
            {
                if (MultiObjectiveFitness != null) return MultiObjectiveFitness[0];
                return null;
            }
        }

        public int? Rank { get; set; }

        public List<double> MultiObjectiveFitness { get; set; }

        public double? CrowdingDistance { get; set; }

        public List<GpProgram> Programs { get; set; }

        public int NumberOfPrograms { get { return Programs.Count; } }
        public List<double> MultiObjectiveValidation { get; set; }

        public List<ProgramStatistic> Statistics
        {
            get
            {
                if (Programs.All(p => p.Statistic == null))
                {
                    return null;
                }
                return Programs.Select(p => p.Statistic).ToList();
            }
            set
            {
                for (int i = 0; i < value.Count; i++)
                {
                    Programs[i].Statistic = value[i];
                }
            }
        }

        public void SetResults(IndividualEvaluationResults results)
        {
            if (results.MultiObjectiveValidation != null && MultiObjectiveValidation == null)
            {
                MultiObjectiveValidation = results.MultiObjectiveValidation.ToList();
            }
            if (results.ProgramStatistics != null && Statistics == null)
            {
                Statistics = results.ProgramStatistics;
            }
            if (results.MultiObjectiveFitness != null && MultiObjectiveFitness == null)
            {
                MultiObjectiveFitness = results.MultiObjectiveFitness.ToList();
            }
        }
    }
}