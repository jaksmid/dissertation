using System;
using Metadata.Attributes;

namespace Metadata.Distance.HungarianAlgorithm
{
    public interface IGpDummyDistance<T> where T : AttributeMetadata
    {
        double GetDistance(AttributeMetadata left, AttributeMetadata right, Func<T, T, double> nonDummyDistance);
    }

    class GpDummyDistanceFromAttribute<T> : IGpDummyDistance<T> where T : AttributeMetadata
    {
        public T Attribute { get; set; }

        public GpDummyDistanceFromAttribute(T attribute)
        {
            Attribute = attribute;
        }

        public double GetDistance(AttributeMetadata left, AttributeMetadata right, Func<T, T, double> nonDummyDistance)
        {
            if (left is DummyAttribute)
            {
                return nonDummyDistance(Attribute, (T)right);
            }
            return nonDummyDistance((T)left, Attribute);
        }
    }
}