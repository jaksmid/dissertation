using System.Collections.Generic;
using GeneticProgramming.Server.Core.GeneticProgramming;

namespace GeneticProgramming.Server.Core.Operators
{
    /// <summary>
    /// Int type operator template
    /// </summary>
    public class IntTemplate:BaseOperatorTemplate
    {
        public IntTemplate(string label, List<string> arguments) : base(label, arguments)
        {
        }

        /// <summary>
        /// Get operator instance
        /// </summary>
        /// <returns>Operator instance</returns>
        public override Operator GetOperatorInstance()
        {
            var op = new Operator {BaseOperator = this};
            int max = int.Parse(Arguments[2]);
            int min = int.Parse(Arguments[1]);
            op.Value = Rnd.Next(min,max).ToString();
            return op;
        }
    }
}