using System;
using System.Collections.Generic;
using System.Linq;
using Accord.Math;
using Accord.Statistics.Testing;
using Newtonsoft.Json;

namespace Metadata.Attributes
{
    public class CategoricalMetadata : AttributeMetadata
    {
        public double ChiSquareUniformDistribution { get; set; }

        public bool UniformDiscrete { get; set; }

        public double RationOfDistinguishingCategoriesByKolmogorovSmirnoffSlashChiSquare { get; set; }
        public double RationOfDistinguishingCategoriesByUtest { get; set; }

        public override void ComputeAttributeTargetDependentMetadata(List<double> attributeValues, List<double> targetValues)
        {
            var keyValueList = attributeValues.Select((t, i) => new KeyValuePair<double, double>(t, targetValues[i])).ToList();
            var distinctValues = attributeValues.Distinct().ToList();
            var nrAttrWithChangingTargetDistributionKs = 0;
            var nrAttrWithChangingTargetDistributionU = 0;
            var rest=targetValues.ToArray();
            foreach (var attributeValue in distinctValues)
            {
                var subSample = keyValueList.Where(kvp => kvp.Key == attributeValue).Select(kvp => kvp.Value).ToArray();

                if (ForRegression)
                {
                    var ksTest = new TwoSampleKolmogorovSmirnovTest(subSample, rest);
                    if (ksTest.Significant) nrAttrWithChangingTargetDistributionKs++;
                }
                if (rest.Length==0) continue;
                try
                {
                    var uTest=new MannWhitneyWilcoxonTest(subSample, rest);
                    if (uTest.Significant) nrAttrWithChangingTargetDistributionU++;
                }
                catch (Exception e)
                {
                    int x = 1;
                    //ignore lib exception
                }
            }
            if (!ForRegression)
            {
                var counts = new Dictionary<double, int>();
                var subSampleCounts = new Dictionary<double, int>();
                var targetDistinct = targetValues.Distinct().ToList();
                foreach (var distinctValue in targetDistinct)
                {
                    counts.Add(distinctValue, keyValueList.Count(x => x.Value == distinctValue));
                }
                foreach (var attributeValue in distinctValues)
                {
                    subSampleCounts.Clear();
                    foreach (var distinctValue in targetDistinct)
                    {
                        subSampleCounts.Add(distinctValue, keyValueList.Count(x => x.Value == distinctValue && x.Key==attributeValue));
                    }
                    int fullSampleSize = counts.Sum(x => x.Value);
                    int subSampleSize = subSampleCounts.Sum(x => x.Value);
                    var ratio = fullSampleSize/(double)subSampleSize;
                    var expectedCounts = counts.OrderBy(x => x.Key).Select(x =>(double)(int)(x.Value/ratio)).ToArray();
                    var sumSampleArray = subSampleCounts.OrderBy(x => x.Key).Select(x => (double)x.Value).ToArray();
                    var chiSquareTest = new ChiSquareTest(expectedCounts, sumSampleArray, targetDistinct.Count() - 1);
                    if (chiSquareTest.Significant) nrAttrWithChangingTargetDistributionKs++;
                }
            }
            RationOfDistinguishingCategoriesByKolmogorovSmirnoffSlashChiSquare =
                (double)nrAttrWithChangingTargetDistributionKs/
                distinctValues.Count();
            

            RationOfDistinguishingCategoriesByUtest=(double)nrAttrWithChangingTargetDistributionU /
                                                                   distinctValues.Count();
            
            
        }

        public void ComputeAttributeDependentMetadata(List<double> attributeValues)
        {
            double chiSquare = 0;
            double expectedCount = attributeValues.Count / (double)Distinct;
            var groups = attributeValues.GroupBy(x=>x);
            foreach (var group in groups)
            {
                var square = Math.Pow(expectedCount - group.Count(), 2);
                var toAdd = square/expectedCount;
                chiSquare += toAdd;
            }
            ChiSquareUniformDistribution = chiSquare;
            var chiTest=new ChiSquareTest(ChiSquareUniformDistribution, (int)Distinct-1);
            UniformDiscrete = !chiTest.Significant;
        }

        [JsonIgnore]
        public override string Type
        {
            get { return "Categorical"; }
        }

        public CategoricalMetadata(List<double?> values, List<double> targetValues, bool forRegression):base(values, targetValues, forRegression)
        {
            var nonMissing = values.Where(x => x.HasValue).Select(y => y.Value).ToList();
            ComputeAttributeDependentMetadata(nonMissing);
        }

        /// <summary>
        /// For Deserialization Purposes
        /// </summary>
        public CategoricalMetadata(){}

        public static List<Double?> RecastCategoricalsToReals(List<string> values)
        {
            var toReturn = new List<Double?>();
            var distinctValues = new Dictionary<string, double>();
            double i = 0;
            foreach (var value in values)
            {
                if (IsMissingValue(value))
                {
                    toReturn.Add(null);
                    continue;
                }
                if (!distinctValues.ContainsKey(value))
                {
                    distinctValues.Add(value, i);
                    i++;
                }
                toReturn.Add(distinctValues[value]);
            }
            return toReturn;
        }
    }
}