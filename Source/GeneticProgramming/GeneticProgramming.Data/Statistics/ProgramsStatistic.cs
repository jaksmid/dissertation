namespace GeneticProgramming.Data.Statistics
{
    public class ProgramsStatistic
    {
        /// <summary>
        /// average number of nodes
        /// </summary>
        private double _averageNumberOfNodes;

        /// <summary>
        /// Average depth
        /// </summary>
        private double _averageDepth;

        /// <summary>
        /// Average width
        /// </summary>
        private double _averageWidth;


        /// <summary>
        /// Average depth
        /// </summary>
        public double AverageDepth
        {
            get { return _averageDepth; }
            set { _averageDepth = value; }
        }

        /// <summary>
        /// Average width
        /// </summary>
        public double AverageWidth
        {
            get { return _averageWidth; }
            set { _averageWidth = value; }
        }

        /// <summary>
        /// average number of nodes
        /// </summary>
        public double AverageNumberOfNodes
        {
            get { return _averageNumberOfNodes; }
            set { _averageNumberOfNodes = value; }
        }
    }
}