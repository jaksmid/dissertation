using System.Collections.Generic;
using System.Linq;
using Metadata.Distance;
using Metadata.Ranking;
using Metadata.Results;

namespace Metadata.Prediction
{
    public class KnnPredictor : IRankingPredictor
    {
        private int _k;
        private ResultsProvider _provider;
        private DistanceMatrix _distanceMatrix;

        public KnnPredictor(int k, ResultsProvider provider, DistanceMatrix distanceMatrix)
        {
            K = k;
            Provider = provider;
            DistanceMatrix = distanceMatrix;
        }

        public int K
        {
            get { return _k; }
            set { _k = value; }
        }

        public ResultsProvider Provider
        {
            get { return _provider; }
            set { _provider = value; }
        }

        public DistanceMatrix DistanceMatrix
        {
            get { return _distanceMatrix; }
            set { _distanceMatrix = value; }
        }

        public IEnumerable<RankingResult> GetPredictionsForDataset(string datasetName, bool filterByCommonAgent=false)
        {
            var toReturn = new List<RankingResult>();
            var rankAverage=new Dictionary<string, double>();
            var modelOccurences=new Dictionary<string, int>();
            IEnumerable<DistanceEntry> nearestNeigbors;
            if (!filterByCommonAgent)
            {
                nearestNeigbors=DistanceMatrix.SortByDistance(datasetName).Where(a=>Provider.Datasets.Contains(a.TargetName)).Take(K);
            }
            else
            {
                var resultsForOrigin = Provider.GetResultsForDataset(datasetName);
                nearestNeigbors = DistanceMatrix.SortByDistance(datasetName).Where(a => Provider.Datasets.Contains(a.TargetName) && Provider.GetResultsForDataset(a.TargetName).Any(target=>resultsForOrigin.Any(origin=>origin.AgentType==target.AgentType))).Take(K);
            }
            foreach (DistanceEntry nearestNeigbor in nearestNeigbors)
            {
                int order = 0;
                foreach (var result in Provider.GetResultsForDataset(nearestNeigbor.TargetName).OrderByDescending(d=>d.Accuracy))
                {
                    order++;
                    if (!modelOccurences.ContainsKey(result.AgentType))
                    {
                        modelOccurences.Add(result.AgentType, 0);
                        rankAverage.Add(result.AgentType, 0);
                    }
                    modelOccurences[result.AgentType]++;
                    rankAverage[result.AgentType] += order;
                }
            }
            foreach (var modelOccurence in modelOccurences)
            {
                rankAverage[modelOccurence.Key] = rankAverage[modelOccurence.Key] / modelOccurence.Value;
            }
            int nrOfModels = modelOccurences.Count;
            var byEstRank = rankAverage.OrderBy(a => a.Value).ToList();
            for (int i = 0; i < nrOfModels; i++)
            {
                toReturn.Add(new RankingResult(byEstRank[i].Key, i + 1, datasetName, nrOfModels));
            }
            return toReturn;
        }

        public List<string> GetDatasets()
        {
            return _distanceMatrix.MetadataNames;
        }
    }
}