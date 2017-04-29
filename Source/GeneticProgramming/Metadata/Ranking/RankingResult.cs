using System;

namespace Metadata.Ranking
{
    public class RankingResult
    {
        private string _modelName;
        private string _datasetName;

        public string DatasetName
        {
            get { return _datasetName; }
            set { _datasetName = value; }
        }

        private int _order;
        private int _outOf;

        public RankingResult(string modelName, int order, string datasetName, int outOf)
        {
            ModelName = modelName;
            Order = order;
            _datasetName = datasetName;
            OutOf = outOf;
        }

        public string ModelName
        {
            get { return _modelName; }
            set { _modelName = value; }
        }

        public int Order
        {
            get { return _order; }
            set { _order = value; }
        }

        public int OutOf
        {
            get { return _outOf; }
            set { _outOf = value; }
        }

        public override string ToString()
        {
            return String.Format("{0}-{1}-estimatedOrder{2} out of {3};", DatasetName, ModelName, Order, OutOf);
        }
    }
}
