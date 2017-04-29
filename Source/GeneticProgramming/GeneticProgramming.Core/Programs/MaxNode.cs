using System;
using System.Collections.Generic;
using GeneticProgramming.Data.Contracts;

namespace GeneticProgramming.Core.Programs
{
    public class MaxNode : BinaryNode
    {
        public MaxNode(Program root, Dictionary<string, Func<string, double>> terminalDictionary)
            : base(root, terminalDictionary)
        {

        }

        public override double CurrentValue => Math.Max(_left.CurrentValue,_right.CurrentValue);
    }
}