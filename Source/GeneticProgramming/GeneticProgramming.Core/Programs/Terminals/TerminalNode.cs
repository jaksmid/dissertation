using System;

namespace GeneticProgramming.Core.Programs.Terminals
{
    public class TerminalNode<T> : IProgramNode
    {
        public Func<T, Double> ValueSetter { get; set; }

        public TerminalNode(Func<T, Double> valueSetter, string label, T argument)
        {
            ValueSetter = valueSetter;
            Label = label;
            Argument = argument;
        }

        public string Label { get; set; }

        public T Argument { get; set; }

        public Double CurrentValue
        {
            get { return ValueSetter(Argument); }
        }

        public double Evaluate()
        {
            return CurrentValue;
        }

        public bool RequiresArgument()
        {
            return true;
        }
    }
}
