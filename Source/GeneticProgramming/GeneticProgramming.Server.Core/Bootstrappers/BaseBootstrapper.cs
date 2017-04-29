using System.Collections.Generic;
using GeneticProgramming.Server.Core.GeneticOperators;

namespace GeneticProgramming.Server.Core.Bootstrappers
{
    public abstract class BaseBootstrapper: IBootstrapper
    {

        protected BaseBootstrapper(int populationSize, int populationsCount)
        {
            PopulationSize = populationSize;
            PopulationsCount = populationsCount;
        }

        public int PopulationSize { get; set; }
        public int PopulationsCount { get; set; }

        public abstract void Init(GeneticProgrammingExperiment experiment);

        public abstract List<IGeneticOperator> GeneticOperators(GeneticProgrammingExperiment experiment);
    }
}
