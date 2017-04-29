using System;
using Metadata.Results;

namespace Metadata.Prediction.Evaluation
{
    public class PredictionQualityEvaluator
    {
        private readonly SpearmanCalculator _spearmanCalculator;

        public PredictionQualityEvaluator(ResultsProvider provider)
        {
            Console.WriteLine("Initialized results provider");
            _spearmanCalculator = new SpearmanCalculator(provider);
        }


        public double Evaluate(IRankingPredictor predictor)
        {
            var datasets = predictor.GetDatasets();
            var averageSpearman = _spearmanCalculator.CalculateAverageSpearman(datasets, predictor);
            return averageSpearman;
        }
    }
}
