using System;
using System.Collections.Generic;
using System.Reflection;
using GeneticProgramming.Server.Core.GeneticProgramming;
using GeneticProgramming.Server.Core.Operators;

namespace GeneticProgramming.Server.Core.Serialization
{
    /// <summary>
    /// Serializable representation of population
    /// </summary>
    [Serializable]
    public class PopulationSerializable
    {
        /// <summary>
        /// Experiment name
        /// </summary>
        public string ExperimentName;

        /// <summary>
        /// GEneration number
        /// </summary>
        public int GenerationNumber;

        /// <summary>
        /// Serializable individuals
        /// </summary>
        public IndividualSerializable[] Individuals;
        //public OperatorTemplateSerializable[] OperatorsLeft;
        /// <summary>
        /// Serializable representation of population info
        /// </summary>
        public PopulationInfoSerializable PopInfo;

        /// <summary>
        /// Population number
        /// </summary>
        public int PopulationNumber;

        /// <summary>
        /// Creates serializable representation of population
        /// </summary>
        /// <param name="pop">Non serializable representation of population</param>
        /// <param name="operators">OperatorsLeft used</param>
        public PopulationSerializable(Population pop, Dictionary<BaseOperatorTemplate, OperatorTemplateSerializable> operators)
        {
            GenerationNumber = pop.GenerationNumber;   

            Individuals = new IndividualSerializable[pop.Individuals.Count];
            for (int i = 0; i < Individuals.Length; i++)
            {
                Individuals[i] = new IndividualSerializable(pop.Individuals[i],operators);
            }
        }

        /// <summary>
        /// Gets non serializable representation of population
        /// </summary>
        /// <param name="operators">OperatorsLeft used</param>
        /// <returns>NonSerializable representation of population</returns>
        public Population GetPopulation(Dictionary<OperatorTemplateSerializable, BaseOperatorTemplate> operators)
        {
            var pop = new Population();
            pop.PopulationNumber = PopulationNumber;
            
            foreach (var ind in Individuals)
            {
                Individual i = ind.GetIndividual(operators);
                pop.Individuals.Add(i);
                //if (i.Fitness == null)
            }

            //foreach (var op in pop.OperatorTemplates)
            //{
            //    int arity=op.Arity;
            //    if (!pop.ArityOperators.ContainsKey(arity)) pop.ArityOperators.Add(arity, new List<BaseOperatorTemplate>());
            //    pop.ArityOperators[arity].Add(op);
            //}
            return pop;
        }
    }

    /// <summary>
    /// Serializable representation of individual
    /// </summary>
    [Serializable]
    public class IndividualSerializable
    {
        /// <summary>
        /// Fitness
        /// </summary>
        public double? Fitness;

        /// <summary>
        /// Identification number
        /// </summary>
        public int NumberOfIndividual;

        /// <summary>
        /// Root operator instance
        /// </summary>
        public OperatorSerializable OperatorInstance;

        /// <summary>
        /// Population number
        /// </summary>
        public int PopulationNumber;

        /// <summary>
        /// Number of nodes
        /// </summary>
        public int? NumberOfNodes;

        /// <summary>
        /// Depths
        /// </summary>
        public int? Depth;

        /// <summary>
        /// Widths
        /// </summary>
        public int? Width;

        /// <summary>
        /// Creates Serializable representation of individual
        /// </summary>
        /// <param name="ind">Non Serializable representation of individual</param>
        /// <param name="operators">OperatorsLeft used</param>
        public IndividualSerializable(Individual ind, Dictionary<BaseOperatorTemplate, OperatorTemplateSerializable> operators)
        {
            Fitness = ind.Fitness;
            NumberOfIndividual = ind.NumberOfIndividual;
            PopulationNumber = ind.PopulationNumber;

            //OperatorInstance = new OperatorSerializable(ind.OperatorInstance,operators);
            //NumbersOfNodes = ind.NumbersOfNodes;
            //Widths = ind.Widths;
            //Depths = ind.Depths;
        }

        /// <summary>
        /// Gets nonSerializable representation of individual
        /// </summary>
        /// <param name="operators">OperatorsLeft used</param>
        /// <returns>NonSerializable representation of individual</returns>
        public Individual GetIndividual(Dictionary<OperatorTemplateSerializable, BaseOperatorTemplate> operators)
        {
            Individual ind = new Individual();
            ind.NumberOfIndividual = NumberOfIndividual;
            ind.PopulationNumber = PopulationNumber;
            //ind.OperatorInstance = OperatorInstance.GetOperator(operators);
            //ind.NumbersOfNodes = NumbersOfNodes;
            //ind.Depths = Depths;
            //ind.Widths = Widths;
            return ind;
        }
    }

    /// <summary>
    /// Serializable representation of operator instance
    /// </summary>
    [Serializable]
    public class OperatorSerializable
    {
        /// <summary>
        /// Value
        /// </summary>
        public string Value;

        /// <summary>
        /// Base operator
        /// </summary>
        public OperatorTemplateSerializable BaseOperator;

        /// <summary>
        /// Children
        /// </summary>
        OperatorSerializable[] Children;

        /// <summary>
        /// Creates Serializable representation of operator instance
        /// </summary>
        /// <param name="op">NonSerializable representation of operator instance</param>
        /// <param name="operators">OperatorsLeft used</param>
        public OperatorSerializable(Operator op, Dictionary<BaseOperatorTemplate, OperatorTemplateSerializable> operators)
        {
            Value = op.Value;
            BaseOperator = operators[op.BaseOperator];
            Children = new OperatorSerializable[op.Children.Count];
            for (int i = 0; i < Children.Length; i++) Children[i] = new OperatorSerializable(op.Children[i],operators);
        }

        /// <summary>
        /// Gets NonSerializable representation of operator instance
        /// </summary>
        /// <param name="operators">OperatorsLeft used</param>
        /// <returns>NonSerializable representation of operator instance</returns>
        public Operator GetOperator(Dictionary<OperatorTemplateSerializable, BaseOperatorTemplate> operators)
        {
            Operator op = new Operator();
            op.Value = Value;
            op.Children = new List<Operator>();
            op.BaseOperator = operators[BaseOperator];
            foreach (var ch in Children) op.Children.Add(ch.GetOperator(operators));
            return op;
        }
    }

    /// <summary>
    /// Serializable representation of operator template
    /// </summary>
    [Serializable]
    public class OperatorTemplateSerializable
    {
        /// <summary>
        /// Label
        /// </summary>
        public string Label;

        /// <summary>
        /// Arguments
        /// </summary>
        public string[] Arguments;

        /// <summary>
        /// Type
        /// </summary>
        public string Type;

        /// <summary>
        /// Creates Serializable representation of operator template
        /// </summary>
        /// <param name="optempl">NonSerializable representation of operator template</param>
        public OperatorTemplateSerializable(BaseOperatorTemplate optempl)
        {
            Label = optempl.Label;
            Arguments = optempl.Arguments.ToArray();
            Type = optempl.GetType().FullName;
        }

        /// <summary>
        /// Creates NonSerializable representation of operator template
        /// </summary>
        /// <returns>Serializable representation of operator template</returns>
        public BaseOperatorTemplate GetOperatorTemplate()
        {
            Assembly gpServiceAssembly = Assembly.Load("GPServer");
            var types = gpServiceAssembly.GetTypes();
            //BaseOperatorTemplate optempl = new BaseOperatorTemplate();
            BaseOperatorTemplate optempl=null;
            foreach (var t in types)
            {
                if (t.FullName.StartsWith(String.Format("{0}", Type)))
                {
                    optempl = (BaseOperatorTemplate)Activator.CreateInstance(t);
                }
            }
            optempl.Label = Label;
            optempl.Arguments = new List<string>();
            foreach (var arg in Arguments) optempl.Arguments.Add(arg);
            return optempl;
        }
    }
}