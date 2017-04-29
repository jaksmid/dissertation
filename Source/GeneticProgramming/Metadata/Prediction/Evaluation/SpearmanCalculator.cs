using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.Statistics;
using Metadata.Ranking;
using Metadata.Results;

namespace Metadata.Prediction.Evaluation
{
    public class SpearmanCalculator
    {
        private ResultsProvider _resultsProvider;

        public SpearmanCalculator(ResultsProvider resultsProvider)
        {
            ResultsProvider = resultsProvider;
        }

        public ResultsProvider ResultsProvider
        {
            get { return _resultsProvider; }
            set { _resultsProvider = value; }
        }

        public double CalculateAverageSpearman(IEnumerable<string> datasets, IRankingPredictor predictor)
        {
            double averageSpearman = 0;
            var relevantResults = 0;
            foreach (var dataset in datasets)
            {
                var predictionResults = predictor.GetPredictionsForDataset(dataset, true);
                double? sperman = CalculateSpearman(dataset, predictionResults);
                if (sperman == null) continue;
                relevantResults++;
                averageSpearman += sperman.Value;
                if (Double.IsNaN(averageSpearman))
                {
                    throw new Exception("Average spearman nan");
                }
            }
            averageSpearman = averageSpearman / relevantResults;
            return averageSpearman;
        }

        public double? CalculateSpearman(String datasetName,IEnumerable<RankingResult> estimations, bool ties=true)
        {
            var sortedAndInEstimations=_resultsProvider.GetResultsForDataset(datasetName).Where(a=>estimations.Any(e=>e.ModelName==a.AgentType))
                .OrderByDescending(a => a.Accuracy).Select(a=>a.AgentType).ToList();
            if (sortedAndInEstimations.Count < 2)
            {
                //Spearman not defined
                return null;
            }
            var rankingResults = estimations.Where(a=>sortedAndInEstimations.Contains(a.ModelName)).OrderBy(a=>a.Order).ToList();
            int estimationRank = 1;
            var xValues = new List<double>();
            var yValues = new List<double>();
            foreach (var rankingResult in rankingResults)
            {
                int actualOrder = sortedAndInEstimations.IndexOf(rankingResult.ModelName) + 1;
                if (actualOrder == 0)
                {
                    throw new Exception();
                }
                xValues.Add(estimationRank);
                yValues.Add(actualOrder);
                estimationRank++;
            }
            if (ties)
            {
                return CalculateSpearmanWithTies(xValues, yValues);
            }
            return CalculateSpearmanWithoutTies(xValues, yValues);
        }

        public double CalculateSpearmanWithoutTies(List<double> xValues, List<double> yValues)
        {
            double squreDiff = 0;
            for (int i = 0; i < xValues.Count; i++)
            {
                int x = (int) xValues[i];
                int y = (int) yValues[i];
                squreDiff += Math.Pow(x - y, 2);
            }
           
            var divisor = Math.Pow(xValues.Count, 3) - xValues.Count;
            squreDiff = 1 - ((6 * squreDiff / divisor));
            if (Math.Abs(squreDiff) > 1)
            {
                throw new Exception();
            }
            return squreDiff;
        }

        public double CalculateSpearmanWithTies(List<double> xValues, List<double> yValues)
        {
            return Correlation.Spearman(xValues, yValues);
        }
    }
}
