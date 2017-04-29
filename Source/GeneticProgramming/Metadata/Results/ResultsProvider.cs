using System;
using System.Collections.Generic;
using System.Linq;
using GeneticProgramming.Data.Contracts;
using GeneticProgramming.Data.Dao;
using Metadata.Ranking;

namespace Metadata.Results
{
    public class ResultsProvider
    {
        private readonly IDbEntitiesProvider _entities;

        public ResultsProvider(IDbEntitiesProvider entities)
        {
            _entities = entities;
            RelevantResults = GetRelevantResults();
            Models = GetModels();
            Datasets = GetDatasets();
        }

        private List<BestResult> _relevantResults;
        private List<String> _models;
        private List<String> _datasets;

        public List<BestResult> RelevantResults
        {
            get { return _relevantResults; }
            set { _relevantResults = value; }
        }

        public List<string> Models
        {
            get { return _models; }
            set { _models = value; }
        }

        public List<string> Datasets
        {
            get { return _datasets; }
            set { _datasets = value; }
        }

        public List<string> ValidationDatasets => _datasets.Where((x, i) => i % 2 == 0).ToList();

        public List<string> TrainingDatasets => _datasets.Except(ValidationDatasets).ToList();

        private List<BestResult> GetRelevantResults()
        {
            return _entities.BestResults.ToList();
        }

        private List<String> GetModels()
        {
            return RelevantResults.Select(a => a.AgentType).Distinct().ToList();
        }

        private List<String> GetDatasets()
        {
            return RelevantResults.Select(a => a.DatasetName.ToString()).Distinct().ToList();
        }

        public List<BestResult> GetResultsForDataset(string dataset)
        {
            return RelevantResults.Where(d => d.DatasetName.ToString() == dataset).ToList();
        }


        public IEnumerable<RankingResult> GetScores(IEnumerable<string> datasets)
        {
            var toReturn = new List<RankingResult>();
            foreach (string dataset in datasets)
            {
                var byError = GetResultsForDataset(dataset).OrderByDescending(d=>d.Accuracy);
                int rank = 1;
                foreach (var availableResult in byError)
                {
                    toReturn.Add(new RankingResult(availableResult.AgentType,rank,dataset,byError.Count()));
                    rank++;
                }
            }
            return toReturn;
        }
    }
}
