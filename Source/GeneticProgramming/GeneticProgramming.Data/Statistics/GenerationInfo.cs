using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GeneticProgramming.Data.Statistics
{
    /// <summary>
    /// Generation info
    /// </summary>
    [DataContract]
    public class GenerationInfo
    {
        /// <summary>
        /// Best fitness
        /// </summary>
        [DataMember]
        public double BestFitness { get; set; }

        /// <summary>
        /// Average fitness
        /// </summary>
        [DataMember]
        public double AverageFitness { get; set; }

        [DataMember]
        public List<ProgramsStatistic> ProgramStatistics { get; set; }
    }
}