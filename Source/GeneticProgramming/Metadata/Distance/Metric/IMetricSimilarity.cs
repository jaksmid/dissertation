namespace Metadata.Distance.Metric
{
    public interface IMetricSimilarity
    {
        double ComputeMetricSimilarity(DistanceMatrix a, DistanceMatrix b);
    }
}
