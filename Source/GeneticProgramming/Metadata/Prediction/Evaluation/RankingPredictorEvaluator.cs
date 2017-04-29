using System;
using System.Collections.Generic;
using Metadata.Global;
using Metadata.Import;
using Metadata.Results;

namespace Metadata.Prediction.Evaluation
{
    public class RankingPredictorEvaluator
    {
        public MetadataCollection TrainingMetadata { get; }
        public MetadataCollection ValidationMetadata { get; }
        private readonly PredictionQualityEvaluator _qualityEvaluator;
        public DbMetadataImporter MetadataImporter { get; set; }
        public ResultsProvider Provider { get; set; }

        public RankingPredictorEvaluator(DbMetadataImporter metadataImporter, ResultsProvider resultsProvider)
        {
            MetadataImporter = metadataImporter;
            Provider = resultsProvider;
            _qualityEvaluator = new PredictionQualityEvaluator(Provider);
            TrainingMetadata = metadataImporter.GetMetadata(new HashSet<string>(Provider.TrainingDatasets));
            ValidationMetadata = metadataImporter.GetMetadata(new HashSet<string>(Provider.ValidationDatasets));
            Console.WriteLine("Initialized global opt fitness");
        }

        private double Evaluate(IRankingPredictor predictor)
        {
            var result = _qualityEvaluator.Evaluate(predictor);
            return result;
        }

        public double GetTrainingResults(Func<MetadataCollection, IRankingPredictor> createPredictor)
        {
            var trainingPredictor = createPredictor(TrainingMetadata);
            return Evaluate(trainingPredictor);
        }

        public double GetTrainingResults(Func<List<string>, IRankingPredictor> createPredictor)
        {
            var trainingPredictor = createPredictor(Provider.TrainingDatasets);
            return Evaluate(trainingPredictor);
        }


        public double GetValidationResults(Func<MetadataCollection, IRankingPredictor> createPredictor)
        {
            var validationPredictor = createPredictor(ValidationMetadata);
            return Evaluate(validationPredictor);
        }

        public double GetValidationResults(Func<List<string>, IRankingPredictor> createPredictor)
        {
            var validationPredictor = createPredictor(Provider.ValidationDatasets);
            return Evaluate(validationPredictor);
        }
    }
}