using System;
using System.Collections.Generic;
using GeneticProgramming.ComputationNode.Configuration.ExperimentSettings;
using Metadata.Import;
using Metadata.Prediction;

namespace GeneticProgramming.ComputationNode.Tasks.Subtasks
{
    public class BaselineSubtask: BaseSubTask
    {
        public BaselineSettings Setting { get; set; }
       

        public BaselineSubtask(BaselineSettings setting):
            base(new GlobalMetadataSettings(GlobalMetadataInclusion.DontInclude))
        {
            Setting = setting;
        }

        public override string RunExperimentWithOpenConnection()
        {
            var trainingResults = QualityEvaluator.GetTrainingResults(CreatePredictorFunction());
            var validationResults = QualityEvaluator.GetValidationResults(CreatePredictorFunction());
            return ("{ \"Training\":" + trainingResults + " ,\"Validation\":"+ validationResults + "}");
        }

        public Func<List<string>, IRankingPredictor> CreatePredictorFunction()
        {
            return datasets => new BaselinePredictor(ResultsProvider, datasets);
        }
    }
}
