using System.Runtime.Serialization;

namespace GeneticProgramming.Data.Statistics
{
    [DataContract]
    public class ProgramStatistic
    {
        /// <summary>
        /// average number of nodes
        /// </summary>
        private int? _numberOfNodes;

        /// <summary>
        /// Average depth
        /// </summary>
        private double? _depth;

        /// <summary>
        /// Average width
        /// </summary>
        private double? _width;


        /// <summary>
        /// Average depth
        /// </summary>
        [DataMember]
        public double? Depth
        {
            get { return _depth; }
            set { _depth = value; }
        }

        /// <summary>
        /// Average width
        /// </summary>
        [DataMember]
        public double? Width
        {
            get { return _width; }
            set { _width = value; }
        }

        /// <summary>
        /// average number of nodes
        /// </summary>
        [DataMember]
        public int? NumberOfNodes
        {
            get { return _numberOfNodes; }
            set { _numberOfNodes = value; }
        }
    }
}