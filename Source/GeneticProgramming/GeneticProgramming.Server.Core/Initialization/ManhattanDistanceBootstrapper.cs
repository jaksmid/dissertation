using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using GeneticProgramming.Server.Core.GeneticProgramming;
using GeneticProgramming.Server.Core.Programs;

namespace GeneticProgramming.Server.Core.Initialization
{
    public abstract class ManhattanDistanceBootstrapper : IProgramInitializator
    {
        public ProgramTypeSet TypeSet { get; set; }
        public double Rescaling { get; set; }
        public List<double> Weights { get; set; }
        public abstract string[] Labels { get; }

        public List<GpProgram> CreatePrograms(int maxDepth)
        {
            var gpProgram = new GpProgram();
            var terminals = TypeSet.GetTerminals;
            var minus = TypeSet.GetFunctions.First(x => x.Label == "-");
            var times = TypeSet.GetFunctions.First(x => x.Label == "*");
            var plus = TypeSet.GetFunctions.First(x => x.Label == "+");
            var abs = TypeSet.GetFunctions.First(x => x.Label == "abs");
            var doubleTemplate = terminals.First(x => x.Label == "double");
            List<Operator> parts = new List<Operator>();
            for (int i = 0; i < Labels.Length; i++)
            {
                var currentLabel = Labels[i];
                var correspondingTemplate = terminals.First(x => x.Label == currentLabel);
                var left = correspondingTemplate.GetOperatorInstance();
                left.Value = "0";
                var right = correspondingTemplate.GetOperatorInstance();
                right.Value = "1";
                var minusInstance = minus.GetOperatorInstance();
                minusInstance.Children = new List<Operator> { left, right };
                var absInstance = abs.GetOperatorInstance();
                absInstance.Children = new List<Operator> {minusInstance};
                var weight = Weights[i];
                var weightOp = doubleTemplate.GetOperatorInstance();
                weightOp.Value = weight.ToString(CultureInfo.InvariantCulture);
                var timesInstance = times.GetOperatorInstance();
                timesInstance.Children = new List<Operator> { weightOp, absInstance };
                parts.Add(timesInstance);
            }
            var currentRoot = parts.First();
            Queue<Operator> rest = new Queue<Operator>(parts.Skip(1));
            while (rest.Count > 0)
            {
                var another = rest.Dequeue();
                var plusInstance = plus.GetOperatorInstance();
                plusInstance.Children = new List<Operator> { currentRoot, another };
                currentRoot = plusInstance;
            }
            //var finalWeight = doubleTemplate.GetOperatorInstance();
            //finalWeight.Value = Rescaling.ToString(CultureInfo.InvariantCulture);
            //var finalTimesInstance = times.GetOperatorInstance();
            //finalTimesInstance.Children = new List<Operator> { finalWeight, currentRoot };
            gpProgram.OperatorInstance = currentRoot;
            return new List<GpProgram> { gpProgram };
        }
    }
}