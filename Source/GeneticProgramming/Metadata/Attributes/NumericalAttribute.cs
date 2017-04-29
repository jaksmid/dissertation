using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Accord.Statistics.Analysis;
using Accord.Statistics.Distributions.Univariate;
using Accord.Statistics.Testing;
using MathNet.Numerics.Statistics;
using Newtonsoft.Json;

namespace Metadata.Attributes
{
    public  class NumericalAttribute : AttributeMetadata
    {
        public NumericalAttribute() { }

        public bool IsUniform { get; set; }

        public bool IntegersOnly { get; set; }

        public double Min { get; set; }
        public double Max { get; set; }
        public double Kurtosis { get; set; }
        public double Mean { get; set; }
        public double Skewness { get; set; }
        public double StandardDeviation { get; set; }
        public double Variance { get; set; }

        public double Mode { get; set; }

        public double Median { get; set; }

        public double ValueRange { get; set; }

        public double LowerOuterFence { get; set; }

        public double HigherOuterFence { get; set; }

        public double HigherQuartile { get; set; }

        public double LowerQuartile { get; set; }

        public double HigherConfidence { get; set; }

        public double LowerConfidence { get; set; }

        public double PositiveCount { get; set; }

        public double PositivePercentage { get; set; }

        public double NegativeCount { get; set; }

        public double NegativePercentage { get; set; }

        public bool HasNegativeValues { get; set; }

        public bool HasPositiveValues { get; set; }

        public void ComputeAttributeDependentMetadata(List<double> attributeValues)
        {
            var statistics = new DescriptiveStatistics(attributeValues);
            Min = statistics.Minimum;
            Max = statistics.Maximum;
            ValueRange = Max - Min;
            PositiveCount = attributeValues.Count(x => x >= 0);
            PositivePercentage = (double)PositiveCount/attributeValues.Count;
            NegativeCount = attributeValues.Count - PositiveCount;
            NegativePercentage = (double)NegativeCount / attributeValues.Count;
            HasNegativeValues = NegativeCount > 0;
            HasPositiveValues = PositiveCount > 0;
            Kurtosis = statistics.Kurtosis;
            Mean = statistics.Mean;
            Skewness = statistics.Skewness;
            StandardDeviation = statistics.StandardDeviation;
            Variance = statistics.Variance;
            var valueArray = attributeValues.ToArray();
            Mode = Accord.Statistics.Tools.Mode(valueArray);
            Median = Accord.Statistics.Tools.Median(valueArray);
            var analysis = new DescriptiveAnalysis(valueArray);
            LowerOuterFence = analysis.OuterFences[0].Min;
            HigherOuterFence = analysis.OuterFences[0].Max;
            LowerConfidence = analysis.Confidence[0].Min;
            HigherConfidence = analysis.Confidence[0].Max;
            LowerQuartile = analysis.Quartiles[0].Min;
            HigherQuartile = analysis.Quartiles[0].Max;
            if (!IntegersOnly)
            {
                var ksTest = new KolmogorovSmirnovTest(attributeValues.ToArray(),
                    new UniformContinuousDistribution(attributeValues.Min(), attributeValues.Max()));
                IsUniform = !ksTest.Significant;
            }
            else
            {
                double chiSquare = 0;
                var distinctValues = attributeValues.Distinct();
                double expectedCount = attributeValues.Count / (double)distinctValues.Count();
                foreach (var distinctValue in distinctValues)
                {
                    int count = attributeValues.Count(d => Math.Abs(d - distinctValue) < 0.1);
                    var square = Math.Pow(expectedCount - count, 2);
                    var toAdd = square / expectedCount;
                    chiSquare += toAdd;
                }
                var chiTest=new ChiSquareTest(chiSquare, distinctValues.Count() - 1);
                IsUniform = !chiTest.Significant;                
            }
        }

        [JsonIgnore]
        public override string Type
        {
            get
            {
                if (IntegersOnly) return "Integer";
                return "Real";
            }
        }

        public NumericalAttribute(List<double?> values,  List<double> targetValues, bool forRegression, bool integersOnly):base(values, targetValues, forRegression)
        {
            IntegersOnly = integersOnly;
            var nonMissing = values.Where(x => x.HasValue).Select(y => y.Value).ToList();
            ComputeAttributeDependentMetadata(nonMissing);
        }

        public static List<Double?> RecastToReals(List<string> values)
        {
            var toReturn = new List<Double?>();
            foreach (var value in values)
            {
                if (IsMissingValue(value))
                {
                    toReturn.Add(null);
                    continue;
                }
                toReturn.Add(Double.Parse(value, NumberStyles.Any, CultureInfo.InvariantCulture));
            }
            return toReturn;
        }
    }
}