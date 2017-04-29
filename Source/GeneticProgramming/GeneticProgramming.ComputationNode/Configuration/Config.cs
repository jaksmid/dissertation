using System.Configuration;
using System.IO;

namespace GeneticProgramming.ComputationNode.Configuration
{
    public class Config
    {
        public string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public static string ResultsDirectory
        {
            get { return "Results" + Path.DirectorySeparatorChar; }
        }
    }
}
