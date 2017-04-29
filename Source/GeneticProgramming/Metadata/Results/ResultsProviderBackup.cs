//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Metadata.Ranking;

//namespace Metadata.Results
//{
//    public class ResultsProvider
//    {
//        private DiplomovaPraceEntities _entities=new DiplomovaPraceEntities();

//        public ResultsProvider()
//        {
//            RelevantResults = GetRelevantResults();
//            Models = GetModels();
//            Datasets = GetDatasets();
//        }

//        private List<bestResults2MetadataAvailable_View> _relevantResults;
//        private List<String> _models;
//        private List<String> _datasets;

//        public List<bestResults2MetadataAvailable_View> RelevantResults
//        {
//            get { return _relevantResults; }
//            set { _relevantResults = value; }
//        }

//        public List<string> Models
//        {
//            get { return _models; }
//            set { _models = value; }
//        }

//        public List<string> Datasets
//        {
//            get { return _datasets; }
//            set { _datasets = value; }
//        }

//        public List<string> ValidationDatasets
//        {
//            get { return Datasets.Where((x, i) => i%3 == 0).ToList(); }
//        }

//        public List<string> TrainingDatasets
//        {
//            get { return _datasets.Except(ValidationDatasets).ToList(); }
//            get { return _datasets.Except(ValidationDatasets).ToList(); }
//        }

//        private List<bestResults2MetadataAvailable_View> GetRelevantResults()
//        {
//            return _entities.bestResults2MetadataAvailable_View.ToList();
//        }

//        private List<String> GetModels()
//        {
//            return RelevantResults.Select(a => a.agentType).Distinct().ToList();
//        }

//        private List<String> GetDatasets()
//        {
//            return RelevantResults.Select(a => a.Source).Distinct().ToList();
//        }

//        public List<bestResults2MetadataAvailable_View> GetResultsForDataset(string dataset)
//        {
//            return RelevantResults.Where(d => d.Source == dataset).ToList();
//        }


//        public IEnumerable<RankingResult> GetScores()
//        {
//            var toReturn = new List<RankingResult>();
//            foreach (string dataset in Datasets)
//            {
//                var byError = GetResultsForDataset(dataset).OrderBy(d=>d.MinError);
//                int rank = 1;
//                foreach (var availableResult in byError)
//                {
//                    toReturn.Add(new RankingResult(availableResult.agentType,rank,dataset,byError.Count()));
//                    rank++;
//                }
//            }
//            return toReturn;
//        }
//    }
//}
