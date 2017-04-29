using GeneticProgramming.ComputationNode.Configuration.ExperimentSettings;
using Metadata.Distance;
using Metadata.Distance.HungarianAlgorithm;
using Metadata.Global;
using Metadata.Import;
using Metadata.Prediction;

namespace GeneticProgramming.ComputationNode.Tasks.Subtasks
{
    public class MasterThesisAlignmentTask: BaseSubTask
    {
        public MasterThesisAlignmentSettings MasterThesisAlignmentSettings { get; set; }
        public DummyDistanceFactory DummyDistanceFactory { get; set; }

        public MasterThesisAlignmentTask(MasterThesisAlignmentSettings masterThesisAlignmentSettings) :
            base(new GlobalMetadataSettings(GlobalMetadataInclusion.DontInclude))
        {
            MasterThesisAlignmentSettings = masterThesisAlignmentSettings;
            //DummyDistanceFactory = dummyDistanceFactory;
        }

        public override string RunExperimentWithOpenConnection()
        {
            var trainingResults = QualityEvaluator.GetTrainingResults(CreatePredictor);
            var validationResults = QualityEvaluator.GetValidationResults(CreatePredictor);
            return ("{ \"Training\":" + trainingResults + " ,\"Validation\":" + validationResults + "}");
        }

        public IRankingPredictor CreatePredictor(MetadataCollection metadata)
        {
            var distanceMatrix = new DistanceMatrix(metadata, new MasterThesisMetric());
            MasterThesisAlignmentSettings.Kernelization.FixMatrix(distanceMatrix);
            var predictor = new KnnPredictor(3, ResultsProvider, distanceMatrix);
            return predictor;
        }
    }
}
