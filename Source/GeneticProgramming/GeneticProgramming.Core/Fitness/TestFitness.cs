using System;
using System.Collections.Generic;
using GeneticProgramming.Core.Programs;
using GeneticProgramming.Core.Programs.Terminals;
using GeneticProgramming.Data.Contracts;

namespace GeneticProgramming.Core.Fitness
{
    public class TestFitness: IFitness
    {
        public List<double> Evaluate(Program[] programs)
        {
            return Eval(programs[0], 0);
        }

        public List<double> Eval(Program p, double offset)
        {
            var currentValues = new CurrrentValueTuple<double>();
            var terminalDictionary = new Dictionary<string, Func<string, double>>
            {
                {"x", s => currentValues.CurrentValueLeft},
                {"y", s => currentValues.CurrentValueRight},
             
            };
            var root = ProgramConverter.Convert(p, terminalDictionary);
            double penalty = 0;
            for (double x = 0; x < 100; x++)
            {
                for (double y = 0; y < 100; y++)
                {
                    var left = x + offset;
                    var right = y + offset;
                    currentValues.CurrentValueLeft = left;
                    currentValues.CurrentValueRight = right;
                    double expected = (left + 2*right) + 5;
                    penalty += Math.Abs(expected - root.CurrentValue);
                }
            }
            var result = 100 - (penalty/10000);
            return new List<double> { result };
        }

        public List<double> Validate(Program[] programs)
        {
            return Eval(programs[0], 0.5);
        }
    }
}
