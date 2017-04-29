using System.Runtime.Serialization;

namespace GeneticProgramming.Data.Contracts
{
    /// <summary>
    /// Basic information
    /// </summary>
    [DataContract]
    public class BasicInfo
    {
        /// <summary>
        /// Number of populations
        /// </summary>
        private int _numberOfPopulations;

        /// <summary>
        /// Population size
        /// </summary>
        private int _populationSize;

        /// <summary>
        /// Number of populations
        /// </summary>
        [DataMember]
        public int NumberOfPopulations
        {
            get { return _numberOfPopulations; }
            set { _numberOfPopulations = value; }
        }

        /// <summary>
        /// Population size
        /// </summary>
        [DataMember]
        public int PopulationSize
        {
            get { return _populationSize; }
            set { _populationSize = value; }
        }
    }
}