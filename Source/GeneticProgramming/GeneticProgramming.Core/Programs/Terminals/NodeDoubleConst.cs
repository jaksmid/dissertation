using System;

namespace GeneticProgramming.Core.Programs.Terminals
{
    public class NodeDoubleConst:IProgramNode
    {
        private readonly double _constant;

        public NodeDoubleConst(Double constant)
        {
            _constant = constant;
        }

        public double CurrentValue
        {
            get { return _constant; }
        }

        public bool RequiresArgument()
        {
            return false;
        }
    }
}
