using System.IO;
using GeneticProgramming.ComputationNode.Configuration.ExperimentSettings;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GeneticProgramming.ComputationNode.Configuration
{
    public class Utils
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(EvolutionOptSettings));

        public static JArray DeserializeJSONFromFile(string filename)
        {
            Logger.Debug("Reading json from file: "+filename);
            JArray result;
            using (StreamReader file = File.OpenText(filename))
            using (var reader = new JsonTextReader(file))
            {
                result = (JArray) JToken.ReadFrom(reader);
            }
            Logger.Debug("Finished reading json from file: " + filename);
            return result;
        }

        public static JArray DeserializeJSON(string jsonString)
        {
            Logger.Debug("Reading json from string");
            var result = JToken.Parse(jsonString);
            if (result is JArray)
            {
                return (JArray) result;
            }
            var toArray = new JArray(result);
            return toArray;
        }
    }
}
