using System;
using Metadata.Attributes;

namespace Metadata.Distance.HungarianAlgorithm
{
    public class ConstantGpDummyDistance<T> : IGpDummyDistance<T> where T : AttributeMetadata
    {
        public double Value { get; set; }

        public ConstantGpDummyDistance()
        {
            Value = 20;
        }

        public double GetDistance(AttributeMetadata left, AttributeMetadata right, Func<T, T, double> nonDummyDistance)
        {
            return Value;
        }
    }
}