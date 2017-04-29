using System;
using System.Collections.Generic;
using GeneticProgramming.Data.Contracts;

namespace GeneticProgramming.Core.Programs
{
    public class DivisionNode : BinaryNode
    {
        public DivisionNode(Program root, Dictionary<string, Func<string, double>> terminalDictionary)
            : base(root, terminalDictionary)
        {
        }

        public override double CurrentValue
        {
            get
            {
                double rightop = _right.CurrentValue;
                if (rightop == 0) return 0;
                var n = _left.CurrentValue/rightop;
                return n;
            }
        }
    }
}