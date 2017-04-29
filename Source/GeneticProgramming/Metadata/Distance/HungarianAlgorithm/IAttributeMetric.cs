using Metadata.Attributes;

namespace Metadata.Distance.HungarianAlgorithm
{
    public interface IAttributeMetric
    {
        double GetDistance(AttributeMetadata attributeA, AttributeMetadata attributeB);
    }
}
