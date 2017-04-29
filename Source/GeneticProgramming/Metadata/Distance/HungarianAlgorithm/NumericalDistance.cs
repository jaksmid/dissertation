using System;
using System.Collections.Generic;
using Metadata.Attributes;

namespace Metadata.Distance.HungarianAlgorithm
{
    public class NumericalDistance:IAttributeMetric
    {
        public IDummyDistance<NumericalAttribute> NumericalDummyDistance { get; set; }
        public WeightedPNormDistance<NumericalAttribute> Distance { get; set; }

        public NumericalDistance(List<double> weights, int? p, IDummyDistance<NumericalAttribute> numericalDummyDistance)
        {
            NumericalDummyDistance = numericalDummyDistance;
            Distance = new WeightedPNormDistance<NumericalAttribute>(weights, p);
        }

        public double GetDistance(AttributeMetadata attributeA, AttributeMetadata attributeB)
        {
            if (attributeA is DummyAttribute || attributeB is DummyAttribute)
            {
                return NumericalDummyDistance.GetDistance(attributeA, attributeB, Distance, Selectors);
            }
            var attA = (NumericalAttribute)attributeA;
            var attB = (NumericalAttribute)attributeB;
            return Distance.MeasureDistance(Selectors, attA, attB);
        }

        public static int NumberOfCoeeficients
        {
            get { return Selectors.Count; }
        }

        private static readonly List<Func<NumericalAttribute, double>> Selectors = new List<Func<NumericalAttribute, double>>
        {
            x=>x.Min,
            x=>x.Max,
            x=>x.CovarianceWithTarget,
            x=>x.Entropy,
            x=>x.PearsonCorrellationCoefficient,
            x=>x.Kurtosis,
            x=>x.Variance,
            x=>x.Skewness,
            x=>x.Mean,
            x=>x.Distinct,
            x=>x.AverageClassCount,
            x=>x.AveragePercentageOfClass,
            x=>x.LeastFequentClassCount,
            x=>x.MedianClassCount,
            x=>x.MedianClassPercentage,
            x=>x.MissingValuesCount,
            x=>x.ModeClassCount,
            x=>x.ModeClassPercentage,
            x=>x.MostFequentClassCount,
            x=>x.NonMissingValuesCount,
            x=>x.PercentageOfLeastFrequentClass,
            x=>x.PercentageOfMissing,
            x=>x.PercentageOfMostFrequentClass,
            x=>x.PercentageOfNonMissing,
            x=>x.ValuesCount,
            x=>x.StandardDeviation,
            x=>x.SpearmanCorrelationCoefficient,
            x=>Convert.ToDouble(x.IntegersOnly),
            x=>Convert.ToDouble(x.IsUniform),
            x=>Convert.ToDouble(x.MissingValues),
            x=>Convert.ToDouble(x.HasNegativeValues),
            x=>Convert.ToDouble(x.HasPositiveValues),
            x=>Convert.ToDouble(x.ForRegression),
            x=>x.Mode,
            x=>x.Median,
            x=>x.ValueRange,
            x=>x.LowerOuterFence,
            x=>x.HigherOuterFence,
            x=>x.LowerQuartile,
            x=>x.HigherQuartile,
            x=>x.HigherConfidence,
            x=>x.LowerConfidence,
            x=>x.PositiveCount,
            x=>x.PositivePercentage,
            x=>x.NegativeCount,
            x=>x.NegativePercentage,
        };

        private List<Func<CategoricalMetadata, double>> _selectors;
    }
}
