using System;
using System.Collections.Generic;
using GeneticProgramming.Server.Core.Operators;

namespace GeneticProgramming.ComputationNode.Configuration.ExperimentSettings.GenProgramming
{
    public class PopulationTemplate
    {
        public int PopulationsCount { get; set; }
        public List<List<OperatorTemplate>> TypeSets { get; set; }
    }

    public class OperatorTemplate
    {
        public string Type { get; set; }
        public string Label { get; set; }

        public List<string> Arguments { get; set; }
    }

    public static class BaseoperatorTemplateFactory
    {
        public static BaseOperatorTemplate CreateTemplate(OperatorTemplate template)
        {
            switch (template.Type.ToLower())
            {
                case "int": return new IntTemplate(template.Label, template.Arguments);
                case "base": return new BaseOperatorTemplate(template.Label, template.Arguments);
                case "double": return new DoubleTemplate(template.Label, template.Arguments);
                default:
                    throw new ArgumentException();
            }
        }
    }
}
