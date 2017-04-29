using System;
using System.Collections.Generic;
using GeneticProgramming.Server.Core.GeneticProgramming;

namespace GeneticProgramming.Server.Core.Operators
{
    /// <summary>
    /// Double type operator template
    /// </summary>
    public class DoubleTemplate:BaseOperatorTemplate
    {
        public DoubleTemplate(string label, List<string> arguments) : base(label, arguments)
        {
        }

        /// <summary>
        /// Get operator instance
        /// </summary>
        /// <returns>Operator instance</returns>
        public override Operator GetOperatorInstance()
        {
            var op = new Operator {BaseOperator = this};
            Double max = Double.Parse(Arguments[2]);
            Double min = Double.Parse(Arguments[1]);
            op.Value = (Rnd.NextDouble() * (max - min) + min).ToString();
            return op;
        }
    }
}