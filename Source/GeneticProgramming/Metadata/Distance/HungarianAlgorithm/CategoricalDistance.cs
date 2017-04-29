using System;
using System.Collections.Generic;
using Metadata.Attributes;

namespace Metadata.Distance.HungarianAlgorithm
{
    public class CategoricalDistance:IAttributeMetric
    {
        public IDummyDistance<CategoricalMetadata> CategoricalDummyDistance { get; set; }
        public WeightedPNormDistance<CategoricalMetadata> Distance { get; set; }

        public CategoricalDistance(List<double> weights, int? p, IDummyDistance<CategoricalMetadata> categoricalDummyDistance)
        {
            CategoricalDummyDistance = categoricalDummyDistance;
            Distance = new WeightedPNormDistance<CategoricalMetadata>(weights, p);
        }


        public double GetDistance(AttributeMetadata attributeA, AttributeMetadata attributeB)
        {
            if (attributeA is DummyAttribute || attributeB is DummyAttribute)
            {
                return CategoricalDummyDistance.GetDistance(attributeA, attributeB, Distance, Selectors);
            }
            var attA = (CategoricalMetadata)attributeA;
            var attB = (CategoricalMetadata)attributeB;
            return Distance.MeasureDistance(Selectors, attA, attB);
        }

        public static int NumberOfCoeeficients => Selectors.Count;

        private static readonly List<Func<CategoricalMetadata, double>> Selectors = new List<Func<CategoricalMetadata, double>>
        {
            x=>x.ChiSquareUniformDistribution,
            x=>x.RationOfDistinguishingCategoriesByKolmogorovSmirnoffSlashChiSquare,
            x=>x.RationOfDistinguishingCategoriesByUtest,
            x=>x.CovarianceWithTarget,
            x=>x.Entropy,
            x=>x.PearsonCorrellationCoefficient,
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
            x=>x.SpearmanCorrelationCoefficient,
            x=>Convert.ToDouble(x.UniformDiscrete),
            x=>Convert.ToDouble(x.MissingValues),
            x=>Convert.ToDouble(x.ForRegression),
        }; 
    }
}
