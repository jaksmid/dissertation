using GeneticProgramming.ComputationNode.Tasks.Subtasks;

namespace GeneticProgramming.ComputationNode.Configuration.ExperimentSettings
{
    public interface ISettings
    {
        string CreateExperimentName();

        ISubTask CreateSubtask();

        string ExperimentPrefix { get; }
    }
}
