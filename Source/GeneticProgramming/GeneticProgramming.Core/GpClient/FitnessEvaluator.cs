using System.Collections.Generic;
using GeneticProgramming.Core.Fitness;
using GeneticProgramming.Core.Helpers;
using GeneticProgramming.Data.Contracts;

namespace GeneticProgramming.Core.GpClient
{
    /// <summary>
    /// Genetic programming main class
    /// </summary>
    public class FitnessEvaluator
    {
        public FitnessEvaluator(IFitness fitnessFunction)
        {
            FitnessFunction = fitnessFunction;
        }

        public IFitness FitnessFunction { get; set; }

        /// <summary>
        /// Evaluate program p
        /// </summary>
        /// <param name="p">Program to evaluate</param>
        /// <returns>Program fitness</returns>
        public virtual List<IndividualEvaluationResults> Evaluate(ProgramEnvelope p)
        {
            var result = new IndividualEvaluationResults();
            if (p.Validate)
            {
                var validationResults = FitnessFunction.Validate(p.Programs);
                result.MultiObjectiveValidation = validationResults.ToArray();
            }
            if (p.Evaluate)
            {
                var fitnessResults = FitnessFunction.Evaluate(p.Programs);
                result.MultiObjectiveFitness = fitnessResults.ToArray();
            }
            ProgramHelpers.FillProgramMetadataToResults(result, p);
            return new List<IndividualEvaluationResults> { result};
        }
    }
}
