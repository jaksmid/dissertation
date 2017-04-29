using GeneticProgramming.Server.Core.GeneticProgramming;
using GeneticProgramming.Server.Core.Settings;

namespace GeneticProgramming.Server.Core.Initialization
{
    public interface IInitializationBootstrapper
    {
        void Bootstrap(Populations popsToBootstrap, IGpExperimentSettings settings);
    }
}