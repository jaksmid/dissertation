using System.Collections.Generic;
using Metadata.Ranking;

namespace Metadata.Prediction
{
    public interface IRankingPredictor
    {
        IEnumerable<RankingResult> GetPredictionsForDataset(string datasetName, bool filterByCommonAgent = false);

        /// <summary>
        /// Provides the list of datasets the model was trained on
        /// </summary>
        /// <returns></returns>
        List<string> GetDatasets();
    }
}
