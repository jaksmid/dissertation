using GeneticProgramming.Server.Core.GeneticProgramming;

namespace GeneticProgramming.Server.Core.GeneticOperators.GlobalOperators
{
    /// <summary>
    /// Global genetic operators operating on multiple populations
    /// </summary>
    public interface IGlobalGeneticOperator
    {
        /// <summary>
        /// Modifies populations
        /// </summary>
        /// <param name="populations">Populations to modify</param>
        void ModifyPopolation(Populations populations);
    }
}
