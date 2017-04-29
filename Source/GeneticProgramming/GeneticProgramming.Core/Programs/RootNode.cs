using System;
using System.Collections.Generic;
using GeneticProgramming.Data.Contracts;

namespace GeneticProgramming.Core.Programs
{
    public class RootNode : UnaryNode
    {

        public RootNode(Program root, Dictionary<string, Func<string, double>> terminalDictionary):base(root, terminalDictionary)
        {
        }

        public override double CurrentValue
        {
            get { return Math.Sqrt(Math.Abs(_child.CurrentValue)); }
        }
    }
}