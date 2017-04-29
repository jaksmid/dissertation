using System;
using System.Collections.Generic;
using GeneticProgramming.Data.Contracts;

namespace GeneticProgramming.Core.Programs
{
    public class LogNode : UnaryNode
    {

        public LogNode(Program root, Dictionary<string, Func<string, double>> terminalDictionary)
            : base(root, terminalDictionary)
        {
            
        }

        public override double CurrentValue
        {
            get
            {
                var subtree = _child.CurrentValue;
                if (subtree == 0) return 0;
                var toReturn = Math.Log(Math.Abs(subtree), 2);
                if (Double.IsNaN(toReturn))
                {
                    throw new ArgumentException();
                }
                return toReturn;
            }
        }
    }
}