using System.Collections.Generic;
using System.Linq;
using GeneticProgramming.Data.Statistics;

namespace GeneticProgramming.Server.Core.GeneticProgramming
{
    public class SingleProgramIndividual
    {
        private readonly Individual _individual;

        public SingleProgramIndividual(Individual individual)
        {
            _individual = individual;
            if (individual.Programs == null)
            {
                individual.Programs=new List<GpProgram>(1) {new GpProgram()};
            }
        }

        public Individual Individual { get { return _individual; } }

        /// <summary>
        /// Number of population in wchid individual resides
        /// </summary>
        public int PopulationNumber {
            get { return _individual.PopulationNumber; }
            set { _individual.PopulationNumber=value; }
        }

        /// <summary>
        /// Identification number
        /// </summary>
        public int NumberOfIndividual {
            get
            {
                return _individual.NumberOfIndividual; 
            }
            set
            {
                _individual.NumberOfIndividual = value;
                
            }
        }

        /// <summary>
        /// Fitness
        /// </summary>
        public double? Fitness
        {
            get
            {
                return _individual.Fitness;
            }
        }

        public int? Rank
        {
            get
            {
                return _individual.Rank;
            }
            set
            {
                _individual.Rank = value;

            }
        }

        public List<double> MultiObjectiveFitness
        {
            get
            {
                return _individual.MultiObjectiveFitness;
            }
            set
            {
                _individual.MultiObjectiveFitness = value;

            }
        }

        public double? CrowdingDistance
        {
            get
            {
                return _individual.CrowdingDistance;
            }
            set
            {
                _individual.CrowdingDistance = value;

            }
        }

        private GpProgram First { get { return _individual.Programs.First(); } }

        /// <summary>
        /// Number of nodes
        /// </summary>
        public ProgramStatistic Statistic
        {
            get { return First.Statistic; }
        }

        /// <summary>
        /// Get all nodes of this individual
        /// </summary>
        public List<Operator> GetNodes
        {
            get { return First.OperatorInstance.GetNodes; }
        }

        /// <summary>
        /// GEt inner nodes of this individual
        /// </summary>
        public List<Operator> GetInnerNodes
        {
            get { return First.OperatorInstance.GetInnerNodes; }
        }


        /// <summary>
        /// Get leaves of this individual
        /// </summary>
        public List<Operator> GetLeaves
        {
            get { return First.OperatorInstance.GetLeaves; }
        }

        /// <summary>
        /// Root operator instance
        /// </summary>
        public Operator OperatorInstance
        {
            get { return First.OperatorInstance; }
            set { First.OperatorInstance = value; }
        }
    }
}