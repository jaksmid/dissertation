using System.Collections.Generic;
using System.Runtime.Serialization;
using GeneticProgramming.Data.Contracts;

namespace GeneticProgramming.Data.Statistics
{
    /// <summary>
    /// Population info
    /// </summary>
    [DataContract]
    public class PopulationInfo
    {
        /// <summary>
        /// Generation number
        /// </summary>
        private uint _generationNumber = 1;

        /// <summary>
        /// Population size
        /// </summary>
        private int _popSize;

        /// <summary>
        /// Best individual fitness
        /// </summary>
        private double _bestIndividualFitness;

        /// <summary>
        /// Average fitness
        /// </summary>
        private double _averageFitness;

        /// <summary>
        /// Best individual
        /// </summary>
        private ProgramEnvelope _bestProgram;

        /// <summary>
        /// Parent history
        /// </summary>
        private PopulationInfo _parent;

        /// <summary>
        /// Fitness distribution
        /// </summary>
        public Dictionary<double, int> FitnessDistribution = new Dictionary<double, int>();

        public List<ProgramsStatistic> ProgramStatistics { get; set; }

            /// <summary>
        /// Population size
        /// </summary>
        [DataMember]
        public int PopSize
        {
            get { return _popSize; }
            set { _popSize = value; }
        }

        /// <summary>
        /// Average fitness
        /// </summary>
        [DataMember]
        public double AverageFitness
        {
            get { return _averageFitness; }
            set { _averageFitness = value; }
        }

        /// <summary>
        /// Best individual
        /// </summary>
        [DataMember]
        public ProgramEnvelope BestProgram
        {
            get { return _bestProgram; }
            set { _bestProgram = value; }
        }

        [DataMember]
        public List<ProgramEnvelope> FirstParetoFront { get; set; }

        /// <summary>
        /// Parent history
        /// </summary>
        [DataMember]
        public PopulationInfo Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        /// <summary>
        /// Best fitness
        /// </summary>
        [DataMember]
        public double BestIndividualFitness
        {
            get { return _bestIndividualFitness; }
            set { _bestIndividualFitness = value; }
        }

        /// <summary>
        /// Generation number
        /// </summary>
        [DataMember]
        public uint GenerationNumber
        {
            get { return _generationNumber; }
            set { if (value > 0) _generationNumber = value; }
        }
    }
}