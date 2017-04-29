using System;
using System.Collections.Generic;
using GeneticProgramming.Server.Core.GeneticProgramming;
using GeneticProgramming.Server.Core.Helpers;

namespace GeneticProgramming.Server.Core.Operators
{
    /// <summary>
    /// Base operator template
    /// </summary>
    public class BaseOperatorTemplate:IOperator
    {
        public BaseOperatorTemplate(string label, List<string> arguments)
        {
            _label = label;
            _arguments = arguments;
        }

        /// <summary>
        /// Label of operator (int,...)
        /// </summary>
        private string _label;

        /// <summary>
        /// Argument for instance creation
        /// </summary>
        private List<string> _arguments = new List<string>();

        /// <summary>
        /// Random number generator
        /// </summary>
        public Random Rnd = RandomHelpers.rnd;

        /// <summary>
        /// Label of operator (int,...)
        /// </summary>
        public string Label
        {
            get { return _label; }
            set { _label = value; }
        }

        /// <summary>
        /// Arity of operator
        /// </summary>
        public int Arity
        {
            get { return int.Parse(_arguments[0]); }
        }

        /// <summary>
        /// Argument for instance creation
        /// </summary>
        public List<string> Arguments
        {
            get { return _arguments; }
            set { _arguments = value; }
        }

        /// <summary>
        /// Get operator instance
        /// </summary>
        /// <returns>Operator instance</returns>
        public virtual Operator GetOperatorInstance()
        {
            var op = new Operator();
            op.BaseOperator = this;
            op.Value = _label;
            return op;
        }
    }

    /// <summary>
    /// Operator interface
    /// </summary>
    public interface IOperator
    {
        /// <summary>
        /// Get operator instance
        /// </summary>
        /// <returns>Operator instance</returns>
        Operator GetOperatorInstance();
    }
}