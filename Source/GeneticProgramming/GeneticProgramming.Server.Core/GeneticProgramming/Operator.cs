using System.Collections.Generic;
using GeneticProgramming.Server.Core.Operators;

namespace GeneticProgramming.Server.Core.GeneticProgramming
{
    /// <summary>
    /// Operator instance
    /// </summary>
    public class Operator
    {
        /// <summary>
        /// Value
        /// </summary>
        string value;

        /// <summary>
        /// Template, type
        /// </summary>
        BaseOperatorTemplate _baseOperator;

        /// <summary>
        /// Children
        /// </summary>
        List<Operator> _children=new List<Operator>();

        /// <summary>
        /// Parent
        /// </summary>
        Operator _parent;

        /// <summary>
        /// Parent
        /// </summary>
        public Operator Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        /// <summary>
        /// Arity
        /// </summary>
        public int Arity
        {
            get { return _baseOperator.Arity; }
        }

        /// <summary>
        /// All child nodes
        /// </summary>
        public List<Operator> GetNodes
        {
            get
            {
                List<Operator> n = new List<Operator>();
                n.Add(this);
                foreach (var op in _children)
                {
                    
                    n.AddRange(op.GetNodes);
                }
                return n;
            }
        }

        /// <summary>
        /// Gets child inner nodes
        /// </summary>
        public List<Operator> GetInnerNodes
        {
            get
            {
                List<Operator> n = new List<Operator>();
                if (this._children.Count != 0)
                {
                    n.Add(this);
                    foreach (var op in _children)
                    {

                        n.AddRange(op.GetInnerNodes);
                    }
                }
                return n;
            }
        }

        /// <summary>
        /// Get child leaves below this node
        /// </summary>
        public List<Operator> GetLeaves
        {
            get
            {
                List<Operator> n = new List<Operator>();
                if (this._children.Count == 0) n.Add(this);
                else
                {
                    foreach (var op in _children)
                    {

                        n.AddRange(op.GetLeaves);
                    }
                }
                return n;
            }
        }

        /// <summary>
        /// Value
        /// </summary>
        public string Value
        {
            get { return value; }
            set { this.value = value; }
        }

        /// <summary>
        /// Template, type
        /// </summary>
        public BaseOperatorTemplate BaseOperator
        {
            get { return _baseOperator; }
            set { _baseOperator = value; }
        }

        /// <summary>
        /// Label
        /// </summary>
        public string Label
        {
            get { return _baseOperator.Label; }            
        }

        /// <summary>
        /// Children
        /// </summary>
        public List<Operator> Children
        {
            get { return _children; }
            set { _children = value; }
        }

    }
}