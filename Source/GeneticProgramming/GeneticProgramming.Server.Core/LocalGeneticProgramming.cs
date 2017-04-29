using GeneticProgramming.Data.Contracts;
using GeneticProgramming.Server.Core.Helpers;
using GeneticProgramming.Server.Core.Settings;

namespace GeneticProgramming.Server.Core
{
    public class LocalGeneticProgramming : IGeneticProgramming
    {
        private readonly string _identifier;
        private object _lock = new object();

        public GeneticProgrammingExperiment Experiment { get; set; }

        public LocalGeneticProgramming(IGpExperimentSettings settings)
        {
            Experiment = new GeneticProgrammingExperiment(settings);
            _identifier = settings.ExperimentName;
        }

        public ProgramEnvelope GetIndividual()
        {
            lock (_lock)
            {
                var toReturn = ProgramEnvelopeFactory.CreateProgramEnvelope(Experiment.GetIndividual(), _identifier);
                return toReturn;
            }
        }

        public ProgramEnvelope GetSpecificIndividual(int generation, int populationNumber, int individualNumber)
        {
            lock (_lock)
            {
                var toReturn = ProgramEnvelopeFactory.CreateProgramEnvelope(Experiment.GetSpecificIndividual(generation, populationNumber, individualNumber), _identifier);
                return toReturn;
            }
        }

        public void RateIndividual(IndividualEvaluationResults result)
        {
            lock (_lock)
            {
                Experiment.RateIndividual(result);
            }
        }
    }
}