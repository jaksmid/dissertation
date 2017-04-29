using GeneticProgramming.Data.Contracts;
using GeneticProgramming.Server.Core.GeneticProgramming;

namespace GeneticProgramming.Server.Core.Helpers
{
    public class ProgramFactory
    {
        /// <summary>
        /// Constructor from operator instance
        /// </summary>
        public static Program CreateProgram(Operator op)
        {
            var toReturn = new Program
            {
                Label = op.Label,
                Value = op.Value,
                Children = new Program[op.Children.Count]
            };
            for (int i = 0; i < toReturn.Children.Length; i++)
            {
                toReturn.Children[i] = CreateProgram(op.Children[i]);
            }
            return toReturn;
        }
    }
}