using System.Linq;
using GeneticProgramming.Data.Contracts;
using GeneticProgramming.Server.Core.GeneticProgramming;

namespace GeneticProgramming.Server.Core.Helpers
{
    public class ProgramEnvelopeFactory
    {
        /// <summary>
        /// Creates program envelope
        /// </summary>
        /// <param name="task">From individual</param>
        /// <param name="experimentIdentifier"></param>
        public static ProgramEnvelope CreateProgramEnvelope(RateIndividualTask task, string experimentIdentifier)
        {
            if (task == null)
            {
                return null;
            }
            var ind = task.IndividualToRate;
            var toReturn = new ProgramEnvelope
            {
                PopulationNumber = ind.PopulationNumber,
                ExperimentIdentifier = experimentIdentifier,
                NumberOfIndividual = ind.NumberOfIndividual,
                GenerationNumber = ind.GenerationNumber,
                Programs = ind.Programs.Select(p => ProgramFactory.CreateProgram(p.OperatorInstance)).ToArray(),
                Evaluate = task.Evaluate,
                Validate = task.Validate,
                TaskId = task.Id,
                ComputeStatistics = task.ComputeStatistics

            };
            return toReturn;
        }

        /// <summary>
        /// Creates program envelope
        /// </summary>
        /// <param name="ind">Individual</param>
        /// <param name="experimentIdentifier"></param>
        public static ProgramEnvelope CreateProgramEnvelope(Individual ind, string experimentIdentifier)
        {
            if (ind == null)
            {
                return null;
            }
            var toReturn = new ProgramEnvelope
            {
                PopulationNumber = ind.PopulationNumber,
                ExperimentIdentifier = experimentIdentifier,
                NumberOfIndividual = ind.NumberOfIndividual,
                GenerationNumber = ind.GenerationNumber,
                Programs = ind.Programs.Select(p => ProgramFactory.CreateProgram(p.OperatorInstance)).ToArray(),
                Evaluate = false,
                Validate = false,
                TaskId = -1,
                ComputeStatistics = false
            };
            return toReturn;
        }
    }
}