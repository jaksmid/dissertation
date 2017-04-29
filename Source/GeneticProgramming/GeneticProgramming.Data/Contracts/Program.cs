using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace GeneticProgramming.Data.Contracts
{
    /// <summary>
    /// Program encapsulating operator instances
    /// </summary>
    [DataContract]
    public class Program
    {
        /// <summary>
        /// Label
        /// </summary>
        private string _label = "";

        /// <summary>
        /// Label value
        /// </summary>
        private string _labelvalue = "";

        /// <summary>
        /// Children
        /// </summary>
        private Program[] _children;


        /// <summary>
        /// Label
        /// </summary>
        [DataMember]
        public string Label
        {
            get { return _label; }
            set { _label = value; }
        }

        /// <summary>
        /// Gets or sets label value
        /// </summary>
        [DataMember]
        public string Value
        {
            get { return _labelvalue; }
            set { _labelvalue = value; }
        }

        /// <summary>
        /// Gets or sets children
        /// </summary>
        [DataMember]
        public Program[] Children
        {
            get { return _children; }
            set { _children = value; }
        }

         public List<Program> FirstTypeChildren
        {
            get
            {
                var toReturn=new List<Program>();
                if (!Children.Any())
                {
                    if (Value == "0") toReturn.Add(this);
                }
                else
                {
                    foreach (var child in Children)
                    {
                        toReturn.AddRange(child.FirstTypeChildren);
                    }
                }
                return toReturn;
            }
        }

        public List<Program> Subtree
        {
            get
            {
                var toReturn = new List<Program>();
                toReturn.Add(this);
                foreach (var program in Children)
                {
                    toReturn.AddRange(program.Subtree);
                }
                return toReturn;
            }
        }

         public List<Program> SecondTypeChildren
         {
             get
             {
                 var toReturn = new List<Program>();
                 if (!Children.Any())
                 {
                     if (Value == "1") toReturn.Add(this);
                 }
                 else
                 {
                     foreach (var child in Children)
                     {
                         toReturn.AddRange(child.SecondTypeChildren);
                     }
                 }
                 return toReturn;
             }
         }
    }
}