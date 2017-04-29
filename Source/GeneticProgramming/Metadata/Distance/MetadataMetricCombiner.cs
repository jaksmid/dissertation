using System;
using System.Collections.Generic;
using Metadata.Global;

namespace Metadata.Distance
{
    public class MetadataMetricCombiner: IMetadataMetric
    {
        public List<IMetadataMetric> Metrics { get; set; }
        public List<double> Weights { get; set; }

        public MetadataMetricCombiner(List<IMetadataMetric> metrics, List<double> weights)
        {
            Metrics = metrics;
            Weights = weights;
        }

        public double MeasureDistance(DatasetMetadata datasetA, DatasetMetadata datasetB)
        {
            double result = 0;
            for (int i = 0; i < Math.Min(Weights.Count, Metrics.Count); i++)
            {
                var current = Weights[i]*Metrics[i].MeasureDistance(datasetA, datasetB);
                result += current;
            }
            return result;
        }
    }
}