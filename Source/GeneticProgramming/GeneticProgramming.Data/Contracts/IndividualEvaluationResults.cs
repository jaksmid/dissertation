using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using GeneticProgramming.Data.Statistics;

namespace GeneticProgramming.Data.Contracts
{
    /// <summary>
    /// Individual evaluation results
    /// </summary>
    [DataContract]
    public class IndividualEvaluationResults
    {
        /// <summary>
        /// Popualtion number
        /// </summary>
        [DataMember]
        public int PopulationNumber;

        [DataMember] 
        public int GenerationNumber;

        /// <summary>
        /// Individual number
        /// </summary>
        [DataMember]
        public int IndividualNumber;

        [DataMember]
        public int TaskId { get; set; }

        [DataMember]
        public double[] MultiObjectiveValidation;

        [DataMember]
        public double[] MultiObjectiveFitness;

        /// <summary>
        /// Programs statistics
        /// </summary>
        [DataMember]
        public List<ProgramStatistic> ProgramStatistics;

        [DataMember]
        public string ExperimentIdentifier { get; set; }

        [DataMember]
        public string EvaluatedBy { get; set; }

        public override string ToString()
        {
            int i = 0;
            string evaluation = "";
            if (MultiObjectiveFitness != null)
            {
                evaluation = MultiObjectiveFitness.Aggregate("", (current, d) =>
                {
                    var toReturn = current + (" Fitness " + i + " " + d);
                    i++;
                    return toReturn;
                });
            }
            i = 0;
            string validation = "";
            if (MultiObjectiveValidation != null)
            {
                validation = MultiObjectiveValidation.Aggregate("", (current, d) =>
                     {
                    var toReturn = current + (" Validation " + i + " " + d);
                    i++;
                    return toReturn;
                });
            }
            return string.Format("Experiment: {0}; Generation: {1}; Population: {2}; IndividualNumber: {3}. Evaluation:{4}. Validation {5}.",
                ExperimentIdentifier, GenerationNumber, PopulationNumber, IndividualNumber, evaluation, validation);
        }
    }
}