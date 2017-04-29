using System;
using System.Collections.Generic;
using GeneticProgramming.Data.Contracts;

namespace GeneticProgramming.Core.Programs
{
    public class LessNode : IProgramNode
    {
        private readonly IProgramNode _a;
        private readonly IProgramNode _b;
        private readonly IProgramNode _c;
        private readonly IProgramNode _d;

        public LessNode(Program root, Dictionary<string, Func<string, double>> terminalDictionary)
        {
            _a = ProgramConverter.Convert(root.Children[0],terminalDictionary);
            _b = ProgramConverter.Convert(root.Children[1],terminalDictionary);
            _c = ProgramConverter.Convert(root.Children[1],terminalDictionary);
            _d = ProgramConverter.Convert(root.Children[1],terminalDictionary);
        }

        public double CurrentValue
        {
            get
            {
                if (_a.CurrentValue < _b.CurrentValue)
                {
                    return _c.CurrentValue;
                }
                return _d.CurrentValue;
            }
        }

        public bool RequiresArgument()
        {
            if (!_a.RequiresArgument() && !_b.RequiresArgument())
            {
                if (_a.CurrentValue < _b.CurrentValue)
                {
                    return _c.RequiresArgument();
                }
                return _d.RequiresArgument();
            }
            return true;
        }
    }
}