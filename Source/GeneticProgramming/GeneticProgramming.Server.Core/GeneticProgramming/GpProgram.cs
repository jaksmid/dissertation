using System.Collections.Generic;
using GeneticProgramming.Data.Statistics;

namespace GeneticProgramming.Server.Core.GeneticProgramming
{
    public class GpProgram
    {
        /// <summary>
        /// Root operator instance
        /// </summary>
        Operator _operatorInstance;

        private ProgramStatistic _statistic;

        /// <summary>
        /// Get all nodes of this individual
        /// </summary>
        public List<Operator> GetNodes
        {
            get { return _operatorInstance.GetNodes; }
        }

        /// <summary>
        /// GEt inner nodes of this individual
        /// </summary>
        public List<Operator> GetInnerNodes
        {
            get { return _operatorInstance.GetInnerNodes; }
        }


        /// <summary>
        /// Get leaves of this individual
        /// </summary>
        public List<Operator> GetLeaves
        {
            get { return _operatorInstance.GetLeaves; }
        }

        /// <summary>
        /// Root operator instance
        /// </summary>
        public Operator OperatorInstance
        {
            get { return _operatorInstance; }
            set { _operatorInstance = value; }
        }

        public ProgramStatistic Statistic
        {
            get { return _statistic; }
            set
            {
                _statistic = value;
                
            }
        }
    }
}