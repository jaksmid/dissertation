using System.Collections.Generic;
using GeneticProgramming.Server.Core.Bootstrappers;
using GeneticProgramming.Server.Core.EndCriterions;
using GeneticProgramming.Server.Core.Initialization;

namespace GeneticProgramming.Server.Core.Settings
{
    public interface IGpExperimentSettings
    {
        IBootstrapper Bootstrapper { get; set; }
        IEndCriterion EndCriterion { get; set; }
        IValidationSelector ValidationSelector { get; set; }
        string ExperimentName { get; }
        List<PopulationBootstrapSettings> PopulationsSettings { get; set; }
        bool FitnessCanChange { get; }
        int PopulationCount { get; }
        List<GeneticOperatorTemplate> Operators { get; set; }
        string SettingsToString();
        IInitializationBootstrapper InitializationBootstrapper { get; set; }
        bool MultiObjective { get; set; }
    }
}
