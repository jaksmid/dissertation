using GeneticProgramming.Server.Core.GeneticProgramming;

namespace GeneticProgramming.Server.Core.Bootstrappers
{
    /// <summary>
    /// Operator initialization interface
    /// </summary>
    public interface IBootstrapper
    {
        /// <summary>
        /// Initialize populations
        /// </summary>
        void Init(GeneticProgrammingExperiment experiment);
    }
}
