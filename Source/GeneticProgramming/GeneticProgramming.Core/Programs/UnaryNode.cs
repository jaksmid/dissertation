using System;
using System.Collections.Generic;
using GeneticProgramming.Data.Contracts;

namespace GeneticProgramming.Core.Programs
{
    public abstract class UnaryNode : IProgramNode
    {
        protected readonly IProgramNode _child;

        protected UnaryNode(Program root, Dictionary<string, Func<string, double>> terminalDictionary)
        {
            _child = ProgramConverter.Convert(root.Children[0], terminalDictionary);
        }

        public abstract double CurrentValue { get; }

        public bool RequiresArgument()
        {
            return _child.RequiresArgument();
        }
    }
}
