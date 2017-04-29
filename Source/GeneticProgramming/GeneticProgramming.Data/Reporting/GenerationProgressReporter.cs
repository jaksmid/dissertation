using System.Text;

namespace GeneticProgramming.Data.Reporting
{
    public class GenerationProgressReporter
    {
        private readonly StringBuilder _writer;
        private bool _firstGeneration = true;

        public GenerationProgressReporter(string experimentName)
        {
            _writer = new StringBuilder();
            _firstGeneration = true;
            string toWrite = string.Format("{0}\"experiment\":\"{1}\", \"fitness\":[", "{", experimentName);
            _writer.Append(toWrite);
        }

        public string EndExperiment(string bestIndividual)
        {
            string toWrite = string.Format("],\"best\":{0}{1}", bestIndividual, "}");
            _writer.Append(toWrite);
            return _writer.ToString();
        }

        public string EndExperiment()
        {
            string toWrite = string.Format("]{0}",  "}");
            _writer.Append(toWrite);
            return _writer.ToString();
        }

        public void AddGeneration(IGenerationProgress progress)
        {
            string toWrite = string.Format("{0}{1}", _firstGeneration ? "" : ",", progress.ToJsonString());
            _firstGeneration = false;
            _writer.Append(toWrite);
        }
    }
}
