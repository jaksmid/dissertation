using System.Collections.Generic;
using GeneticProgramming.ComputationNode.Tasks.Subtasks;
using Metadata.Distance.Kernelization;
using Newtonsoft.Json.Linq;

namespace GeneticProgramming.ComputationNode.Configuration.ExperimentSettings
{
    public class MasterThesisAlignmentSettings: BaseSettings
    {
        public IKernelization Kernelization { get; set; }

        public List<KernelizationTypes> KernelizationTypes { get; set; }
        public MasterThesisAlignmentSettings(JToken config): base(config)
        {
            var kernelizationTuple = ConfigParser.ParseKernelization(Config);
            KernelizationTypes = kernelizationTuple.Item1;
            Kernelization = kernelizationTuple.Item2;
        }

        public override ISubTask CreateSubtask()
        {
            return new MasterThesisAlignmentTask(this);
        }

        public override string CreateExperimentName()
        {
            return BuildExperimentName("_"+string.Join(";",KernelizationTypes));
        }

        public override string ExperimentPrefix
        {
            get { return "MasterThesisAlignment"; }
        }
    }
}
