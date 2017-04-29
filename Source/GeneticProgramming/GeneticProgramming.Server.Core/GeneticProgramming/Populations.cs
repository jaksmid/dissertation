using System.Collections.Generic;
using GeneticProgramming.Server.Core.GeneticOperators.GlobalOperators;

namespace GeneticProgramming.Server.Core.GeneticProgramming
{
    /// <summary>
    /// Populations
    /// </summary>
    public class Populations
    {
        /// <summary>
        /// Generation number of all populations
        /// </summary>
        public int GenerationNumber
        {
            get
            {
                if (Islands == null || Islands.Count == 0) return 0;
                return Islands[0].GenerationNumber;
            }
        }

        /// <summary>
        /// All populations
        /// </summary>
        public List<Population> Islands { get; set; }

        /// <summary>
        /// OperatorsLeft by arity
        /// </summary>
        public List<IGlobalGeneticOperator> Operators { get; set; }

        public Populations(GeneticProgrammingExperiment experiment)
        {
            Operators = new List<IGlobalGeneticOperator> { new CleanupQueue(experiment) ,new TriggerNextGenerationInEveryPopulation() };
        }
    }
}