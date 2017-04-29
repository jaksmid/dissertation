using System.Collections.Generic;
using GeneticProgramming.Server.Core.Programs;

namespace GeneticProgramming.Server.Core.Settings
{
    public class PopulationBootstrapSettings
    {
        public int PopulationSize { get; set; }
        public List<ProgramTypeSet> TypeSets { get; set; }

        public PopulationBootstrapSettings(int populationSize, List<ProgramTypeSet> typeSets)
        {
            PopulationSize = populationSize;
            TypeSets = typeSets;
        }
    }
}
