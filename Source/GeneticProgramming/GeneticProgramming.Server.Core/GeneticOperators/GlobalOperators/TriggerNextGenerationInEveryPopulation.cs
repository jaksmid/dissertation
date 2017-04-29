using GeneticProgramming.Server.Core.GeneticProgramming;

namespace GeneticProgramming.Server.Core.GeneticOperators.GlobalOperators
{
    public class TriggerNextGenerationInEveryPopulation: IGlobalGeneticOperator
    {
        public void ModifyPopolation(Populations populations)
        {
            foreach (var population in populations.Islands)
            {
                population.NextGeneration();
            }
        }
    }
}
