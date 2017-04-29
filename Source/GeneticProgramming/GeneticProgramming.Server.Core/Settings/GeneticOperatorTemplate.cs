using System;
using System.Collections.Generic;
using System.Globalization;

namespace GeneticProgramming.Server.Core.Settings
{
    public class GeneticOperatorTemplate
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public List<string> Dependencies { get; set; }
        public Dictionary<string, string> Arguments { get; set; }

        public string GetValue(string name)
        {
            return Arguments[name];
        }

        public int GetIntValue(string name)
        {
            return Int32.Parse(GetValue(name), CultureInfo.InvariantCulture);
        }

        public double GetDoubleValue(string name)
        {
            return double.Parse(GetValue(name), CultureInfo.InvariantCulture);
        }

        public string GetValueOrDefault(string name, string defaultValue)
        {
            if (Arguments.ContainsKey(name))
            {
                return Arguments[name];
            }
            return defaultValue;
        }

        public int GetIntValueOrDefault(string name, int defaultValue)
        {
            if (Arguments.ContainsKey(name))
            {
                return GetIntValue(name);
            }
            return defaultValue;
        }

        public double GetDoubleValueOrDefault(string name, double defaultValue)
        {
            if (Arguments.ContainsKey(name))
            {
                return GetDoubleValue(name);
            }
            return defaultValue;
        }
    }
}
