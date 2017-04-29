using Metadata.Distance;
using Metadata.Global;

namespace GeneticProgramming.Core.Metrics
{
    public class MetricArgumentSwitcher : IMetadataMetric
    {
        private readonly IMetadataMetric _originalMetric;

        public MetricArgumentSwitcher(IMetadataMetric originalMetric)
        {
            _originalMetric = originalMetric;
        }

        public double MeasureDistance(DatasetMetadata datasetA, DatasetMetadata datasetB)
        {
            return _originalMetric.MeasureDistance(datasetB, datasetA);
        }
    }
}
