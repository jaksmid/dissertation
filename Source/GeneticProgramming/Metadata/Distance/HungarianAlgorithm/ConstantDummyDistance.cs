using System;
using System.Collections.Generic;
using Metadata.Attributes;

namespace Metadata.Distance.HungarianAlgorithm
{
    public class ConstantDummyDistance<T> : IDummyDistance<T> where T : AttributeMetadata
    {
        public double Weight { get; set; }

        public ConstantDummyDistance(double weight)
        {
            Weight = weight;
        }

        public double GetDistance(AttributeMetadata left, AttributeMetadata right, WeightedPNormDistance<T> originalDistance, List<Func<T, double>> selectors)
        {
            return Weight*40;
        }
    }
}