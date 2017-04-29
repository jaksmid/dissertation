using System.Collections.Generic;
using System.Linq;
using Metadata.Ranking;
using Metadata.Results;

namespace Metadata.Prediction
{
    public class BaselinePredictor : IRankingPredictor
    {
        public List<string> Datasets { get; set; }
        private readonly Dictionary<string, double> _rankAverage=new Dictionary<string, double>();
        private readonly List<KeyValuePair<string, double>> _orderedResults;
        private readonly int _nrOfModels;

        public BaselinePredictor(ResultsProvider previousResults, List<string> datasets)
        {
            Datasets = datasets;
            ResultsProvider previousResults1 = previousResults;
            var scores=previousResults1.GetScores(Datasets);
            
            var modelOccurences=new Dictionary<string, int>();
            foreach (var model in previousResults1.Models)
            {
                modelOccurences.Add(model,0);
                _rankAverage.Add(model,0);
            }
            foreach (var score in scores)
            {
                modelOccurences[score.ModelName]++;
                _rankAverage[score.ModelName] += score.Order;
            }
            var modelsWithAtLeastOneOccurrence = modelOccurences.Where(x => x.Value > 0).ToList();
            _nrOfModels = modelsWithAtLeastOneOccurrence.Count();
            foreach (var modelOccurence in modelsWithAtLeastOneOccurrence)
            {
                _rankAverage[modelOccurence.Key] = _rankAverage[modelOccurence.Key]/modelOccurence.Value;
            }
            _orderedResults = _rankAverage.OrderBy(a => a.Value).ToList();
        }

        public IEnumerable<RankingResult> GetPredictionsForDataset(string datasetName, bool filterByCommonAgent=false)
        {
            var toReturn=new List<RankingResult>();
            for (int i = 0; i < _nrOfModels; i++)
            {
                toReturn.Add(new RankingResult(_orderedResults[i].Key, i + 1, datasetName, _nrOfModels));
            }
            return toReturn;
        }

        public List<string> GetDatasets()
        {
            return Datasets;
        }
    }
}