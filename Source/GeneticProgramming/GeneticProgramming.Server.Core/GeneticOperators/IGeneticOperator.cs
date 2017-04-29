using GeneticProgramming.Server.Core.GeneticProgramming;

namespace GeneticProgramming.Server.Core.GeneticOperators
{
    /// <summary>
    /// Genetic operator interfcae
    /// </summary>
    public interface IGeneticOperator
    {
        /// <summary>
        /// Modifies population
        /// </summary>
        /// <param name="pop">Population to modify</param>
        void ModifyPopolation(Population pop);
    }
}
