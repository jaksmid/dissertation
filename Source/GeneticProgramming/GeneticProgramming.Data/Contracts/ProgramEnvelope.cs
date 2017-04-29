using System.Runtime.Serialization;

namespace GeneticProgramming.Data.Contracts
{
    /// <summary>
    /// Program envelope encapsulating individual
    /// </summary>
    [DataContract]
    public class ProgramEnvelope
    {
        /// <summary>
        /// Encapsulated program
        /// </summary>
        private Program[] _programs;

        /// <summary>
        /// Individual number
        /// </summary>
        int numberOfIndividual;

        /// <summary>
        /// Population number
        /// </summary>
        int populationNumber;

        /// <summary>
        /// Generation number
        /// </summary>
        [DataMember]
        public int GenerationNumber { get; set; }

        /// <summary>
        /// Generation progress of populations
        /// </summary>
        [DataMember]
        public int GenerationProgress { get; set; }

        [DataMember]
        public int TaskId { get; set; }
        
        /// <summary>
        /// Encapsulated program
        /// </summary>
        [DataMember]
        public Program[] Programs
        {
            get { return _programs; }
            set { _programs = value; }
        }

        /// <summary>
        /// Number of individual
        /// </summary>
        [DataMember]
        public int NumberOfIndividual
        {
            get { return numberOfIndividual; }
            set { numberOfIndividual = value; }
        }

        /// <summary>
        /// Population number
        /// </summary>
        [DataMember]
        public int PopulationNumber
        {
            get { return populationNumber; }
            set { populationNumber = value; }
        }

        [DataMember]
        public string ExperimentIdentifier { get; set; }

        [DataMember]
        public bool Evaluate { get; set; }

        [DataMember]
        public bool Validate { get; set; }

        [DataMember]
        public bool ComputeStatistics { get; set; }

        public override string ToString()
        {
            return string.Format("Experiment: {0}; Generation: {1}; Population: {2}; IndividualNumber: {3}",
                ExperimentIdentifier, GenerationNumber, PopulationNumber, NumberOfIndividual);
        }
    }
}