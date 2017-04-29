using System;
using System.Collections.Generic;
using GeneticProgramming.Data.Contracts;

namespace GeneticProgramming.Core.Programs
{
    public class AbsNode : UnaryNode
    {

        public AbsNode(Program root, Dictionary<string, Func<string, double>> terminalDictionary)
            : base(root, terminalDictionary)
        {

        }

        public override double CurrentValue
        {
            get
            {
                var subtree = _child.CurrentValue;
                return Math.Abs(subtree);
            }
        }
    }
}