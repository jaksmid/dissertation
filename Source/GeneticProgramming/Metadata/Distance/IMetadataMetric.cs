using Metadata.Distance.Metric;
using Metadata.Global;

namespace Metadata.Distance
{
    public interface IMetadataMetric
    {
        double MeasureDistance(DatasetMetadata datasetA, DatasetMetadata datasetB);
    }
}
