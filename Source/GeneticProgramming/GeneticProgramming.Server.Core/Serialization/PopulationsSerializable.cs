using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GeneticProgramming.Server.Core.Bootstrappers;
using GeneticProgramming.Server.Core.GeneticProgramming;
using GeneticProgramming.Server.Core.Operators;

namespace GeneticProgramming.Server.Core.Serialization
{
    /// <summary>
    /// Serializable representation of populations
    /// </summary>
    [Serializable]
    public class PopulationsSerializable
    {
        /// <summary>
        /// Populations stored
        /// </summary>
        public PopulationSerializable[] Islands;

        /// <summary>
        /// OperatorsLeft used
        /// </summary>
        public OperatorTemplateSerializable[] Operators;

        /// <summary>
        /// Creates Serializable representation of populations
        /// </summary>
        /// <param name="pops">NonSerializable representation of populations</param>
        public PopulationsSerializable(Populations pops)
        {
            //InitedBy = pops.InitedBy;
            //ExperimentName = pops.ExperimentName;            

            var operators = new Dictionary<BaseOperatorTemplate, OperatorTemplateSerializable>();
            //TODO move to pop serializable foreach (var op in pops.OperatorTemplates) operators.Add(op, new OperatorTemplateSerializable(op));
            Operators = operators.Values.ToArray();

            Islands = new PopulationSerializable[pops.Islands.Count];
            for (int i = 0; i < Islands.Length; i++) Islands[i] = new PopulationSerializable(pops.Islands[i],operators);
        }

        /// <summary>
        /// Gets NonSerializable representation of populations
        /// </summary>
        /// <returns>NonSerializable representation of populations</returns>
        public Populations GetPopulations()
        {
            var navrat = new Populations(null);
            //navrat.ExperimentName = ExperimentName;
            //navrat.InitedBy = InitedBy;

            var operators = new Dictionary<OperatorTemplateSerializable, BaseOperatorTemplate>();

            foreach (var op in Operators)
            {
                BaseOperatorTemplate opt = op.GetOperatorTemplate();
                //TODO: navrat.OperatorTemplates.Add(opt);
                operators.Add(op, opt);
            }

            Assembly gpServiceAssembly = Assembly.Load("GPServer");
            var types = gpServiceAssembly.GetTypes();
            //BaseOperatorTemplate optempl = new BaseOperatorTemplate();
            IBootstrapper bootstrapper = null;
            foreach (var t in types)
            {
                if (t.FullName.Contains(null))
                {
                    bootstrapper = (IBootstrapper)Activator.CreateInstance(t);
                }
            }

            navrat.Islands = new List<Population>();
            foreach (var isl in Islands) navrat.Islands.Add(isl.GetPopulation(operators));
            //TODO: foreach (var v in navrat.Islands) v.GeneticOperators = bootstrapper.GeneticOperators();
            return navrat;
        }
    }
}