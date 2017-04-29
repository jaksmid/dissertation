using GeneticProgramming.ComputationNode.Tasks.Subtasks;

namespace GeneticProgramming.ComputationNode.Configuration.ExperimentSettings
{
    public class BaselineSettings: BaseSettings
    {
        public override ISubTask CreateSubtask()
        {
            return new BaselineSubtask(this);
        }

        public override string ExperimentPrefix { get { return "Baseline"; } }
    }
}
