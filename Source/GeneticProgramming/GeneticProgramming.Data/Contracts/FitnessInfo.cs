using System.Collections.Generic;
using System.Runtime.Serialization;
using GeneticProgramming.Data.Statistics;

namespace GeneticProgramming.Data.Contracts
{
    /// <summary>
    /// Fitness info
    /// </summary>
    [DataContract]
    public class FitnessInfo
    {
        /// <summary>
        /// Fitness history
        /// </summary>
        [DataMember]
        public List<GenerationInfo> FitnessHistory;

        /// <summary>
        /// Fitness distribution
        /// </summary>
        [DataMember]
        public List<KeyValuePair<double, int>> FitnessDistribution;
    }
}