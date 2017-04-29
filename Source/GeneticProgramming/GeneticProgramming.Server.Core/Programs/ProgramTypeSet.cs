using System.Collections.Generic;
using System.Linq;
using GeneticProgramming.Server.Core.Operators;

namespace GeneticProgramming.Server.Core.Programs
{
    public class ProgramTypeSet
    {
        public ProgramTypeSet( List<BaseOperatorTemplate> operators)
        {
            OperatorTemplates = operators;
            ArityOperators = new Dictionary<int, List<BaseOperatorTemplate>>();
            foreach (var v in operators)
            {
                if (!ArityOperators.ContainsKey(v.Arity)) ArityOperators.Add(v.Arity, new List<BaseOperatorTemplate>());
                ArityOperators[v.Arity].Add(v);
            }
        }

        /// <summary>
        /// OperatorsLeft by arity
        /// </summary>
        public Dictionary<int, List<BaseOperatorTemplate>> ArityOperators { get; set; }

        /// <summary>
        /// Operator templates
        /// </summary>
        public List<BaseOperatorTemplate> OperatorTemplates { get; set; }

        /// <summary>
        /// Terminal set
        /// </summary>
        public List<BaseOperatorTemplate> GetTerminals
        {
            get { return ArityOperators[0]; }
        }

        /// <summary>
        /// Function set
        /// </summary>
        public List<BaseOperatorTemplate> GetFunctions
        {
            get
            {
                var oplist = new List<BaseOperatorTemplate>();
                foreach (var kvp in ArityOperators.Where(kvp => kvp.Key != 0))
                {
                    oplist.AddRange(kvp.Value);
                }
                return oplist;
            }
        }
    }
}