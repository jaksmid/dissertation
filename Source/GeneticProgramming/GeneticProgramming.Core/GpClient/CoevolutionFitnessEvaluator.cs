using System;
using System.Collections.Generic;
using GeneticProgramming.Core.Fitness;
using GeneticProgramming.Core.Helpers;
using GeneticProgramming.Data.Contracts;

namespace GeneticProgramming.Core.GpClient
{
    public class CoevolutionFitnessEvaluator : FitnessEvaluator
    {
        public Func<int, int, int, ProgramEnvelope> GetSpecificFriend { get; set; }

        public CoevolutionFitnessEvaluator(IFitness fitnessFunction, Func<int, int, int, ProgramEnvelope> getSpecificFriend) : base(fitnessFunction)
        {
            GetSpecificFriend = getSpecificFriend; 
        }

        public override List<IndividualEvaluationResults> Evaluate(ProgramEnvelope p)
        {
            var friend = GetSpecificFriend(p.GenerationNumber, (p.PopulationNumber + 1) % 2, p.NumberOfIndividual);
            Program[] programs = new Program[2];
            programs[p.PopulationNumber] = p.Programs[0];
            programs[friend.PopulationNumber] = friend.Programs[0];
            var result = new IndividualEvaluationResults();
            var friendResult = new IndividualEvaluationResults();
            //if (p.NumberOfIndividual == 0)
            //{
            //    var fitnessResults = FitnessFunction.Evaluate(programs);
            //    var validationResults = FitnessFunction.Validate(programs);
            //}
            if (p.Validate)
            {
                var validationResults = FitnessFunction.Validate(programs);
                result.MultiObjectiveValidation = validationResults.ToArray();
                friendResult.MultiObjectiveValidation = result.MultiObjectiveValidation;
            }
            if (p.Evaluate)
            {
                var fitnessResults = FitnessFunction.Evaluate(programs);
                result.MultiObjectiveFitness = fitnessResults.ToArray();
                friendResult.MultiObjectiveFitness = result.MultiObjectiveFitness;
            }
            ProgramHelpers.FillProgramMetadataToResults(result, p);
            friend.ComputeStatistics = p.ComputeStatistics;
            ProgramHelpers.FillProgramMetadataToResults(friendResult, friend);
            return new List<IndividualEvaluationResults> { result, friendResult };
        }
    }
}