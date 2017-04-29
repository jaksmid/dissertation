using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.Statistics;
using Metadata.Attributes;
using Metadata.Global;

namespace Metadata.Distance.HungarianAlgorithm
{
    public class FromAttributeDummyDistanceFactory : DummyDistanceFactory
    {
        public MetadataCollection Metadata { get; set; }

        public readonly CategoricalMetadata CategoricalDummyMetadata;
            //new CategoricalMetadata()
            //{
            //    AverageClassCount = 0.5,
            //    ChiSquareUniformDistribution = 0,
            //    AveragePercentageOfClass = 0.5,
            //    CovarianceWithTarget = 0.5,
            //    Distinct = 0.5,
            //    Entropy = 0.5,
            //    ForRegression = false,
            //    IsTarget = false,
            //    LeastFequentClassCount = 0.5,
            //    MedianClassCount = 0.5,
            //    MedianClassPercentage = 0.5,
            //    MissingValues = false,
            //    MissingValuesCount = 0,
            //    ModeClassCount = 0.5,
            //    ModeClassPercentage = 0.5,
            //    MostFequentClassCount = 0.5,
            //    NonMissingValuesCount = 0.5,
            //    PearsonCorrellationCoefficient = 0.5,
            //    PercentageOfLeastFrequentClass = 0.5,
            //    PercentageOfMissing = 0.5,
            //    PercentageOfMostFrequentClass = 0.5,
            //    PercentageOfNonMissing = 0.5,
            //    RationOfDistinguishingCategoriesByKolmogorovSmirnoffSlashChiSquare = 0,
            //    RationOfDistinguishingCategoriesByUtest = 0,
            //    SpearmanCorrelationCoefficient = 0.5,
            //    UniformDiscrete = false,
            //    ValuesCount = 0
            //};

        public readonly NumericalAttribute NumericalDummyAttribute;
        //{
        //    AverageClassCount = 0.5,
        //    AveragePercentageOfClass = 0.5,
        //    CovarianceWithTarget = 0.5,
        //    Distinct = 0.5,
        //    Entropy = 0.5,
        //    ForRegression = false,
        //    IsTarget = false,
        //    LeastFequentClassCount = 0.5,
        //    MedianClassCount = 0.5,
        //    MedianClassPercentage = 0.5,
        //    MissingValues = false,
        //    MissingValuesCount = 0,
        //    ModeClassCount = 0.5,
        //    ModeClassPercentage = 0.5,
        //    MostFequentClassCount = 0.5,
        //    NonMissingValuesCount = 0.5,
        //    PearsonCorrellationCoefficient = 0.5,
        //    PercentageOfLeastFrequentClass = 0.5,
        //    PercentageOfMissing = 0.5,
        //    PercentageOfMostFrequentClass = 0.5,
        //    PercentageOfNonMissing = 0.5,
        //    SpearmanCorrelationCoefficient = 0.5,
        //    ValuesCount = 0,
        //    Max = 0.5,
        //    HasNegativeValues = false,
        //    HasPositiveValues = true,
        //    HigherConfidence = 0.5,
        //    HigherOuterFence = 0.5,
        //    HigherQuartile = 0.5,
        //    IntegersOnly = true,
        //    IsUniform = false,
        //    Kurtosis = 0.5,
        //    LowerConfidence = 0.5,
        //    LowerOuterFence = 0.5,
        //    LowerQuartile = 0.5,
        //    Mean = 0.5,
        //    Median = 0.5,
        //    Min = 0.5,
        //    Mode = 0.5,
        //    NegativeCount = 0,
        //    NegativePercentage = 0,
        //    PositiveCount = 0.5,
        //    PositivePercentage = 1,
        //    Skewness = 0.5,
        //    StandardDeviation = 0.5,
        //    ValueRange = 0.5,
        //    Variance = 0.5
        //};

        public void SetValueOfCategoricalToMean(Func<CategoricalMetadata, double> getFunction, Action<CategoricalMetadata, double> setAction)
        {
            var values = new List<double>();
            foreach (var datasetMetadata in Metadata.Metadatas)
            {
                values.AddRange(datasetMetadata.CategoricalAttributes.Select(getFunction));
            }
            var mean = values.Median();
            setAction(CategoricalDummyMetadata, mean);
        }

        public void SetValueOfCategoricalToMean(Func<CategoricalMetadata, bool> getFunction, Action<CategoricalMetadata, bool> setAction)
        {
            var values = new List<bool>();
            foreach (var datasetMetadata in Metadata.Metadatas)
            {
                values.AddRange(datasetMetadata.CategoricalAttributes.Select(getFunction));
            }
            var total = values.Count;
            var positive = values.Count(x => x);
            var negative = total - positive;
            var toSet = positive > negative;
            setAction(CategoricalDummyMetadata, toSet);
        }

        public void SetValueOfNumericalToMean(Func<NumericalAttribute, double> getFunction, Action<NumericalAttribute, double> setAction)
        {
            var values = new List<double>();
            foreach (var datasetMetadata in Metadata.Metadatas)
            {
                values.AddRange(datasetMetadata.NumericalAttributes.Select(getFunction));
            }
            var mean = values.Median();
            setAction(NumericalDummyAttribute, mean);
        }

        public void SetValueOfNumericalToMean(Func<NumericalAttribute, bool> getFunction, Action<NumericalAttribute, bool> setAction)
        {
            var values = new List<bool>();
            foreach (var datasetMetadata in Metadata.Metadatas)
            {
                values.AddRange(datasetMetadata.NumericalAttributes.Select(getFunction));
            }
            var total = values.Count;
            var positive = values.Count(x => x);
            var negative = total - positive;
            var toSet = positive > negative;
            setAction(NumericalDummyAttribute, toSet);
        }

        public FromAttributeDummyDistanceFactory(MetadataCollection metadata)
        {
            Metadata = metadata;
            CategoricalDummyMetadata = new CategoricalMetadata();
            SetValueOfCategoricalToMean(x => x.AverageClassCount, (x, y) => x.AverageClassCount = y);
            SetValueOfCategoricalToMean(x => x.ChiSquareUniformDistribution, (x, y) => x.ChiSquareUniformDistribution = y);
            SetValueOfCategoricalToMean(x => x.AveragePercentageOfClass, (x, y) => x.AveragePercentageOfClass = y);
            SetValueOfCategoricalToMean(x => x.CovarianceWithTarget, (x, y) => x.CovarianceWithTarget = y);
            SetValueOfCategoricalToMean(x => x.Distinct, (x, y) => x.Distinct = y);
            SetValueOfCategoricalToMean(x => x.Entropy, (x, y) => x.Entropy = y);
            SetValueOfCategoricalToMean(x => x.LeastFequentClassCount, (x, y) => x.LeastFequentClassCount = y);
            SetValueOfCategoricalToMean(x => x.MedianClassCount, (x, y) => x.MedianClassCount = y);
            SetValueOfCategoricalToMean(x => x.MedianClassPercentage, (x, y) => x.MedianClassPercentage = y);
            SetValueOfCategoricalToMean(x => x.MissingValuesCount, (x, y) => x.MissingValuesCount = y);
            SetValueOfCategoricalToMean(x => x.ModeClassCount, (x, y) => x.ModeClassCount = y);
            SetValueOfCategoricalToMean(x => x.ModeClassPercentage, (x, y) => x.ModeClassPercentage = y);
            SetValueOfCategoricalToMean(x => x.MostFequentClassCount, (x, y) => x.MostFequentClassCount = y);
            SetValueOfCategoricalToMean(x => x.NonMissingValuesCount, (x, y) => x.NonMissingValuesCount = y);
            SetValueOfCategoricalToMean(x => x.PearsonCorrellationCoefficient, (x, y) => x.PearsonCorrellationCoefficient = y);
            SetValueOfCategoricalToMean(x => x.PercentageOfLeastFrequentClass, (x, y) => x.PercentageOfLeastFrequentClass = y);
            SetValueOfCategoricalToMean(x => x.PercentageOfMissing, (x, y) => x.PercentageOfMissing = y);
            SetValueOfCategoricalToMean(x => x.PercentageOfMostFrequentClass, (x, y) => x.PercentageOfMostFrequentClass = y);
            SetValueOfCategoricalToMean(x => x.PercentageOfNonMissing, (x, y) => x.PercentageOfNonMissing = y);
            SetValueOfCategoricalToMean(x => x.RationOfDistinguishingCategoriesByKolmogorovSmirnoffSlashChiSquare, (x, y) => x.RationOfDistinguishingCategoriesByKolmogorovSmirnoffSlashChiSquare = y);
            SetValueOfCategoricalToMean(x => x.RationOfDistinguishingCategoriesByUtest, (x, y) => x.RationOfDistinguishingCategoriesByUtest = y);
            SetValueOfCategoricalToMean(x => x.SpearmanCorrelationCoefficient, (x, y) => x.SpearmanCorrelationCoefficient = y);
            SetValueOfCategoricalToMean(x => x.ValuesCount, (x, y) => x.ValuesCount = y);
            //bool
            SetValueOfCategoricalToMean(x => x.ForRegression, (x, y) => x.ForRegression = y);
            SetValueOfCategoricalToMean(x => x.IsTarget, (x, y) => x.IsTarget = y);
            SetValueOfCategoricalToMean(x => x.MissingValues, (x, y) => x.MissingValues = y);
            SetValueOfCategoricalToMean(x => x.UniformDiscrete, (x, y) => x.UniformDiscrete = y);

            //numerical
            NumericalDummyAttribute = new NumericalAttribute();
            SetValueOfNumericalToMean(x => x.AverageClassCount, (x, y) => x.AverageClassCount = y);
            SetValueOfNumericalToMean(x => x.AveragePercentageOfClass, (x, y) => x.AveragePercentageOfClass = y);
            SetValueOfNumericalToMean(x => x.CovarianceWithTarget, (x, y) => x.CovarianceWithTarget = y);
            SetValueOfNumericalToMean(x => x.Distinct, (x, y) => x.Distinct = y);
            SetValueOfNumericalToMean(x => x.Entropy, (x, y) => x.Entropy = y);
            SetValueOfNumericalToMean(x => x.LeastFequentClassCount, (x, y) => x.LeastFequentClassCount = y);
            SetValueOfNumericalToMean(x => x.MedianClassCount, (x, y) => x.MedianClassCount = y);
            SetValueOfNumericalToMean(x => x.MedianClassPercentage, (x, y) => x.MedianClassPercentage = y);
            SetValueOfNumericalToMean(x => x.MissingValuesCount, (x, y) => x.MissingValuesCount = y);
            SetValueOfNumericalToMean(x => x.ModeClassCount, (x, y) => x.ModeClassCount = y);
            SetValueOfNumericalToMean(x => x.ModeClassPercentage, (x, y) => x.ModeClassPercentage = y);
            SetValueOfNumericalToMean(x => x.MostFequentClassCount, (x, y) => x.MostFequentClassCount = y);
            SetValueOfNumericalToMean(x => x.NonMissingValuesCount, (x, y) => x.NonMissingValuesCount = y);
            SetValueOfNumericalToMean(x => x.PearsonCorrellationCoefficient, (x, y) => x.PearsonCorrellationCoefficient = y);
            SetValueOfNumericalToMean(x => x.PercentageOfLeastFrequentClass, (x, y) => x.PercentageOfLeastFrequentClass = y);
            SetValueOfNumericalToMean(x => x.PercentageOfMissing, (x, y) => x.PercentageOfMissing = y);
            SetValueOfNumericalToMean(x => x.PercentageOfMostFrequentClass, (x, y) => x.PercentageOfMostFrequentClass = y);
            SetValueOfNumericalToMean(x => x.PercentageOfNonMissing, (x, y) => x.PercentageOfNonMissing = y);
            SetValueOfNumericalToMean(x => x.SpearmanCorrelationCoefficient, (x, y) => x.SpearmanCorrelationCoefficient = y);
            SetValueOfNumericalToMean(x => x.ValuesCount, (x, y) => x.ValuesCount = y);
            SetValueOfNumericalToMean(x => x.Max, (x, y) => x.Max = y);
            SetValueOfNumericalToMean(x => x.HigherConfidence, (x, y) => x.HigherConfidence = y);
            SetValueOfNumericalToMean(x => x.HigherOuterFence, (x, y) => x.HigherOuterFence = y);
            SetValueOfNumericalToMean(x => x.HigherQuartile, (x, y) => x.HigherQuartile = y);
            SetValueOfNumericalToMean(x => x.Kurtosis, (x, y) => x.Kurtosis = y);
            SetValueOfNumericalToMean(x => x.LowerConfidence, (x, y) => x.LowerConfidence = y);
            SetValueOfNumericalToMean(x => x.LowerOuterFence, (x, y) => x.LowerOuterFence = y);
            SetValueOfNumericalToMean(x => x.LowerQuartile, (x, y) => x.LowerQuartile = y);
            SetValueOfNumericalToMean(x => x.Mean, (x, y) => x.Mean = y);
            SetValueOfNumericalToMean(x => x.Median, (x, y) => x.Median = y);
            SetValueOfNumericalToMean(x => x.Min, (x, y) => x.Min = y);
            SetValueOfNumericalToMean(x => x.Mode, (x, y) => x.Mode = y);
            SetValueOfNumericalToMean(x => x.NegativeCount, (x, y) => x.NegativeCount = y);
            SetValueOfNumericalToMean(x => x.NegativePercentage, (x, y) => x.NegativePercentage = y);
            SetValueOfNumericalToMean(x => x.PositiveCount, (x, y) => x.PositiveCount = y);
            SetValueOfNumericalToMean(x => x.PositivePercentage, (x, y) => x.PositivePercentage = y);
            SetValueOfNumericalToMean(x => x.Skewness, (x, y) => x.Skewness = y);
            SetValueOfNumericalToMean(x => x.StandardDeviation, (x, y) => x.StandardDeviation = y);
            SetValueOfNumericalToMean(x => x.ValueRange, (x, y) => x.ValueRange = y);
            SetValueOfNumericalToMean(x => x.Variance, (x, y) => x.Variance = y);
            //bool
            SetValueOfNumericalToMean(x => x.ForRegression, (x, y) => x.ForRegression = y);
            SetValueOfNumericalToMean(x => x.IsTarget, (x, y) => x.IsTarget = y);
            SetValueOfNumericalToMean(x => x.MissingValues, (x, y) => x.MissingValues = y);
            SetValueOfNumericalToMean(x => x.HasNegativeValues, (x, y) => x.HasNegativeValues = y);
            SetValueOfNumericalToMean(x => x.HasPositiveValues, (x, y) => x.HasPositiveValues = y);
            SetValueOfNumericalToMean(x => x.IntegersOnly, (x, y) => x.IntegersOnly = y);
            SetValueOfNumericalToMean(x => x.IsUniform, (x, y) => x.IsUniform = y);
        }

        public override IDummyDistance<CategoricalMetadata> GetCategoricalDummyDistance(List<double> weights)
        {
            return new DummyDistanceFromAttribute<CategoricalMetadata>(CategoricalDummyMetadata);
        }

        public override IDummyDistance<NumericalAttribute> GetNumericalDummyDistance(List<double> weights)
        {
            return new DummyDistanceFromAttribute<NumericalAttribute>(NumericalDummyAttribute);
        }

        public override IGpDummyDistance<CategoricalMetadata> GetCategoricalGpDummyDistance()
        {
            return new GpDummyDistanceFromAttribute<CategoricalMetadata>(CategoricalDummyMetadata);
        }

        public override IGpDummyDistance<NumericalAttribute> GetNumericalGpDummyDistance()
        {
            return new GpDummyDistanceFromAttribute<NumericalAttribute>(NumericalDummyAttribute);
        }
    }
}