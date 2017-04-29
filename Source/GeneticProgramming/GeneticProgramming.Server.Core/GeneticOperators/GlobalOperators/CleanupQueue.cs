using GeneticProgramming.Server.Core.GeneticProgramming;

namespace GeneticProgramming.Server.Core.GeneticOperators.GlobalOperators
{
    public class CleanupQueue : IGlobalGeneticOperator
    {
        public GeneticProgrammingExperiment Experiment { get; set; }

        public CleanupQueue(GeneticProgrammingExperiment experiment)
        {
            Experiment = experiment;
        }

        public void ModifyPopolation(Populations populations)
        {
            Experiment.RatingQueue.Cleanup();
        }
    }
}