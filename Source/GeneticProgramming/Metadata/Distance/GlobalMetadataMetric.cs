using System.Collections.Generic;
using Metadata.Distance.HungarianAlgorithm;
using Metadata.Global;

namespace Metadata.Distance
{
    public class GlobalMetadataMetric : IMetadataMetric
    {
        public WeightedPNormFloatDistance Distance { get; set; }

        public GlobalMetadataMetric(List<double> coefficients, int? p)
        {
            Distance = new WeightedPNormFloatDistance(coefficients, p);
        }

        public double MeasureDistance(DatasetMetadata datasetA, DatasetMetadata datasetB)
        {
            var globalA = datasetA.GlobalMetadata.Values;
            var globalB = datasetB.GlobalMetadata.Values;
            return Distance.MeasureDistance(globalA, globalB);
        }
    }
}