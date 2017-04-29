using System;
using System.Collections.Generic;
using GeneticProgramming.Data.Contracts;

namespace GeneticProgramming.Core.Programs
{
    public abstract class BinaryNode : IProgramNode
    {
        protected readonly IProgramNode _left;
        protected readonly IProgramNode _right;

        protected BinaryNode(Program root, Dictionary<string, Func<string, double>> terminalDictionary)
        {
            _left = ProgramConverter.Convert(root.Children[0], terminalDictionary);
            _right = ProgramConverter.Convert(root.Children[1], terminalDictionary);
        }

        public abstract double CurrentValue { get; }

        public bool RequiresArgument()
        {
            return _left.RequiresArgument() || _right.RequiresArgument();
        }
    }
}
