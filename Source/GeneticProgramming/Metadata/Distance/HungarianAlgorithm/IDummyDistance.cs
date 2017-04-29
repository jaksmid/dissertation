using System;
using System.Collections.Generic;
using Metadata.Attributes;

namespace Metadata.Distance.HungarianAlgorithm
{
    public interface IDummyDistance<T> where T:AttributeMetadata
    {
        double GetDistance(AttributeMetadata left, AttributeMetadata right, WeightedPNormDistance<T> originalDistance, List<Func<T, double>> selectors);
    }

    class DummyDistanceFromAttribute<T> : IDummyDistance<T> where T : AttributeMetadata
    {
        public T Attribute { get; set; }

        public DummyDistanceFromAttribute(T attribute)
        {
            Attribute = attribute;
        }

        public double GetDistance(AttributeMetadata left, AttributeMetadata right, WeightedPNormDistance<T> originalDistance, List<Func<T, double>> selectors)
        {
            if (left is DummyAttribute)
            {
                return originalDistance.MeasureDistance(selectors, Attribute, (T) right);
            }
            return originalDistance.MeasureDistance(selectors, (T)left, Attribute);
        }
    }
}