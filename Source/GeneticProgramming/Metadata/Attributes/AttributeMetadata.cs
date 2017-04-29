using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.Statistics;
using Newtonsoft.Json;

namespace Metadata.Attributes
{
    public abstract class AttributeMetadata 
    {
        public double PearsonCorrellationCoefficient { get; set; }
        public double SpearmanCorrelationCoefficient { get; set; }

        public double CovarianceWithTarget { get; set; }

        public double Entropy { get; set; }

        public bool ForRegression { get; set; }

        public bool MissingValues { get; set; }
        [JsonIgnore]
        public virtual string Type
        {
            get { return "Attribute"; }
        }

        public double ValuesCount { get; set; }
        public double NonMissingValuesCount { get; set; }

        public double MissingValuesCount { get; set; }

        public double PercentageOfMissing { get; set; }

        public double PercentageOfNonMissing { get; set; }
        public double Distinct { get; set; }

        public double PercentageOfMostFrequentClass { get; set; }

        public double PercentageOfLeastFrequentClass { get; set; }

        public double AveragePercentageOfClass { get; set; }

        public double AverageClassCount { get; set; }

        public double MostFequentClassCount { get; set; }

        public double LeastFequentClassCount { get; set; }

        public double ModeClassCount { get; set; }

        public double ModeClassPercentage { get; set; }

        public double MedianClassCount { get; set; }

        public double MedianClassPercentage { get; set; }

        public bool IsTarget { get; set; }

        protected AttributeMetadata()
        {

        }

        protected AttributeMetadata(List<double?> values, List<double> targetValues, bool isForRegression)
        {
            ValuesCount = values.Count;
            var nonMissing = values.Where(x => x.HasValue).Select(y => y.Value).ToList();
            NonMissingValuesCount = nonMissing.Count();
            MissingValuesCount = ValuesCount - NonMissingValuesCount;
            MissingValues = MissingValuesCount > 0;
            PercentageOfNonMissing = NonMissingValuesCount / (double)ValuesCount;
            PercentageOfMissing = MissingValuesCount/(double) ValuesCount;
            //Count distincts etc
            var valueCounts = new Dictionary<double, int>();
            foreach (var value in nonMissing)
            {
                if (!valueCounts.ContainsKey(value)) valueCounts.Add(value, 0);
                valueCounts[value]++;
            }
            Distinct = valueCounts.Count;
            MostFequentClassCount = valueCounts.Max(x => x.Value);
            LeastFequentClassCount = valueCounts.Min(x => x.Value);
            AverageClassCount = ValuesCount/(Double)Distinct;
            AveragePercentageOfClass = Distinct/(Double) (ValuesCount);
            PercentageOfMostFrequentClass = MostFequentClassCount/(Double) ValuesCount;
            PercentageOfLeastFrequentClass = LeastFequentClassCount / (Double)ValuesCount;
            var distinctCounts = valueCounts.Select(x => x.Value).ToArray();
            ModeClassCount = Accord.Statistics.Tools.Mode(distinctCounts);
            MedianClassCount = Accord.Statistics.Tools.Median(distinctCounts.Select(x=>(double)(x)).ToArray());
            ModeClassPercentage = ModeClassCount/(double) ValuesCount;
            MedianClassPercentage = MedianClassCount/ValuesCount;

            ForRegression = isForRegression;
            Entropy = Statistics.Entropy(nonMissing);
            if (targetValues != null)
            {
                ComputeAdvancedInfo(values, targetValues);
            }
        }

        public void ComputeAdvancedInfo(List<double?> attributeValue, List<double> targetValues)
        {
            var attrDoubles=new List<double>();
            var targetDoubles=new List<double>();
            int i = 0;
            foreach (var attValue in attributeValue)
            {
                if (attValue != null)
                {
                    attrDoubles.Add(attValue.Value);
                    targetDoubles.Add(targetValues[i]);
                }
                i++;
            }
            ComputeAdvancedInfo(attrDoubles, targetDoubles);
        }

        public void ComputeAdvancedInfo(List<double> attributeValues, List<Double> targetValues)
        {
            PearsonCorrellationCoefficient = Correlation.Pearson(attributeValues, targetValues);
            if (Double.IsNaN(PearsonCorrellationCoefficient))
            {
                throw  new Exception();
            }
            SpearmanCorrelationCoefficient = Correlation.Spearman(attributeValues, targetValues);
            CovarianceWithTarget = attributeValues.Covariance(targetValues);
            ComputeAttributeTargetDependentMetadata(attributeValues,targetValues);
        }

        public static bool IsMissingValue(string toCheck)
        {
            return string.IsNullOrEmpty(toCheck) || toCheck == "?";
        }

        public virtual void ComputeAttributeTargetDependentMetadata(List<double> attributeValues, List<double> targetValues){}
    }
}