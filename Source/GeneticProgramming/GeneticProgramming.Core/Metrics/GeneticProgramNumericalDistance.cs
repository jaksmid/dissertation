using System;
using System.Collections.Generic;
using GeneticProgramming.Data.Contracts;
using Metadata.Attributes;
using Metadata.Distance.HungarianAlgorithm;

namespace GeneticProgramming.Core.Metrics
{
    public class GeneticProgramNumericalDistance : GeneticProgrammingDistance<NumericalAttribute>
    {
        public GeneticProgramNumericalDistance(Program program, IGpDummyDistance<NumericalAttribute> dummyDistance, bool repairToSemimetric) : base(program, dummyDistance, repairToSemimetric)
        {
        }

        public override Dictionary<string, Func<string, double>> GetTerminalDictionary()
        {
            var terminalDictionary = new Dictionary<string, Func<string, double>>
            {
                //Common
                {"PR", s => s == "0"? CurrentValues.CurrentValueLeft.PearsonCorrellationCoefficient : CurrentValues.CurrentValueRight.PearsonCorrellationCoefficient},
                {"SR", s => s == "0"? CurrentValues.CurrentValueLeft.SpearmanCorrelationCoefficient : CurrentValues.CurrentValueRight.SpearmanCorrelationCoefficient},
                {"CT", s => s == "0"? CurrentValues.CurrentValueLeft.CovarianceWithTarget : CurrentValues.CurrentValueRight.CovarianceWithTarget},
                {"EN", s => s == "0"? CurrentValues.CurrentValueLeft.Entropy : CurrentValues.CurrentValueRight.Entropy},
                {"FR", s => s == "0"? (CurrentValues.CurrentValueLeft.ForRegression? 1 : 0) : (CurrentValues.CurrentValueRight.ForRegression? 1 : 0)},
                {"MV", s => s == "0"? (CurrentValues.CurrentValueLeft.MissingValues? 1 : 0) : (CurrentValues.CurrentValueRight.MissingValues? 1 : 0)},
                {"ValuesCount", s => s == "0"? CurrentValues.CurrentValueLeft.ValuesCount : CurrentValues.CurrentValueRight.ValuesCount},
                {"NonMissingValuesCount", s => s == "0"? CurrentValues.CurrentValueLeft.NonMissingValuesCount : CurrentValues.CurrentValueRight.NonMissingValuesCount},
                {"MissingValuesCount", s => s == "0"? CurrentValues.CurrentValueLeft.MissingValuesCount : CurrentValues.CurrentValueRight.MissingValuesCount},
                {"PercentageOfMissing", s => s == "0"? CurrentValues.CurrentValueLeft.PercentageOfMissing : CurrentValues.CurrentValueRight.PercentageOfMissing},
                {"PercentageOfNonMissing", s => s == "0"? CurrentValues.CurrentValueLeft.PercentageOfNonMissing : CurrentValues.CurrentValueRight.PercentageOfNonMissing},
                {"Distinct", s => s == "0"? CurrentValues.CurrentValueLeft.Distinct : CurrentValues.CurrentValueRight.Distinct},
                {"PercentageOfMostFrequentClass", s => s == "0"? CurrentValues.CurrentValueLeft.PercentageOfMostFrequentClass : CurrentValues.CurrentValueRight.PercentageOfMostFrequentClass},
                {"PercentageOfLeastFrequentClass", s => s == "0"? CurrentValues.CurrentValueLeft.PercentageOfLeastFrequentClass : CurrentValues.CurrentValueRight.PercentageOfLeastFrequentClass},
                {"AveragePercentageOfClass", s => s == "0"? CurrentValues.CurrentValueLeft.AveragePercentageOfClass : CurrentValues.CurrentValueRight.AveragePercentageOfClass},
                {"AverageClassCount", s => s == "0"? CurrentValues.CurrentValueLeft.AverageClassCount : CurrentValues.CurrentValueRight.AverageClassCount},
                {"MostFequentClassCount", s => s == "0"? CurrentValues.CurrentValueLeft.MostFequentClassCount : CurrentValues.CurrentValueRight.MostFequentClassCount},
                {"LeastFequentClassCount", s => s == "0"? CurrentValues.CurrentValueLeft.LeastFequentClassCount : CurrentValues.CurrentValueRight.LeastFequentClassCount},
                {"ModeClassCount", s => s == "0"? CurrentValues.CurrentValueLeft.ModeClassCount : CurrentValues.CurrentValueRight.ModeClassCount},
                {"ModeClassPercentage", s => s == "0"? CurrentValues.CurrentValueLeft.ModeClassPercentage : CurrentValues.CurrentValueRight.ModeClassPercentage},
                {"MedianClassCount", s => s == "0"? CurrentValues.CurrentValueLeft.MedianClassCount : CurrentValues.CurrentValueRight.MedianClassCount},
                {"MedianClassPercentage", s => s == "0"? CurrentValues.CurrentValueLeft.MedianClassPercentage : CurrentValues.CurrentValueRight.MedianClassPercentage},

                //Numerical specific
                {"IU", s => s == "0"? (CurrentValues.CurrentValueLeft.IsUniform? 1 : 0) : (CurrentValues.CurrentValueRight.IsUniform? 1 : 0)},
                {"IO", s => s == "0"? (CurrentValues.CurrentValueLeft.IntegersOnly? 1 : 0) : (CurrentValues.CurrentValueRight.IntegersOnly? 1 : 0)},
                {"Min", s => s == "0"? CurrentValues.CurrentValueLeft.Min : CurrentValues.CurrentValueRight.Min},
                {"Max", s => s == "0"? CurrentValues.CurrentValueLeft.Max : CurrentValues.CurrentValueRight.Max},
                {"KURT", s => s == "0"? CurrentValues.CurrentValueLeft.Kurtosis : CurrentValues.CurrentValueRight.Kurtosis},
                {"Mean", s => s == "0"? CurrentValues.CurrentValueLeft.Mean : CurrentValues.CurrentValueRight.Mean},
                {"SK", s => s == "0"? CurrentValues.CurrentValueLeft.Skewness : CurrentValues.CurrentValueRight.Skewness},
                {"SD", s => s == "0"? CurrentValues.CurrentValueLeft.StandardDeviation : CurrentValues.CurrentValueRight.StandardDeviation},
                {"VAR", s => s == "0"? CurrentValues.CurrentValueLeft.Variance : CurrentValues.CurrentValueRight.Variance},
                {"Mode", s => s == "0"? CurrentValues.CurrentValueLeft.Mode : CurrentValues.CurrentValueRight.Mode},
                {"Median", s => s == "0"? CurrentValues.CurrentValueLeft.Median : CurrentValues.CurrentValueRight.Median},
                {"ValueRange", s => s == "0"? CurrentValues.CurrentValueLeft.ValueRange : CurrentValues.CurrentValueRight.ValueRange},
                {"LowerOuterFence", s => s == "0"? CurrentValues.CurrentValueLeft.LowerOuterFence : CurrentValues.CurrentValueRight.LowerOuterFence},
                {"HigherOuterFence", s => s == "0"? CurrentValues.CurrentValueLeft.HigherOuterFence : CurrentValues.CurrentValueRight.HigherOuterFence},
                {"HigherQuartile", s => s == "0"? CurrentValues.CurrentValueLeft.HigherQuartile : CurrentValues.CurrentValueRight.HigherQuartile},
                {"LowerQuartile", s => s == "0"? CurrentValues.CurrentValueLeft.LowerQuartile : CurrentValues.CurrentValueRight.LowerQuartile},
                {"HigherConfidence", s => s == "0"? CurrentValues.CurrentValueLeft.HigherConfidence : CurrentValues.CurrentValueRight.HigherConfidence},
                {"LowerConfidence", s => s == "0"? CurrentValues.CurrentValueLeft.LowerConfidence : CurrentValues.CurrentValueRight.LowerConfidence},
                {"PositiveCount", s => s == "0"? CurrentValues.CurrentValueLeft.PositiveCount : CurrentValues.CurrentValueRight.PositiveCount},
                {"PositivePercentage", s => s == "0"? CurrentValues.CurrentValueLeft.PositivePercentage : CurrentValues.CurrentValueRight.PositivePercentage},
                {"NegativeCount", s => s == "0"? CurrentValues.CurrentValueLeft.NegativeCount : CurrentValues.CurrentValueRight.NegativeCount},
                {"NegativePercentage", s => s == "0"? CurrentValues.CurrentValueLeft.NegativePercentage : CurrentValues.CurrentValueRight.NegativePercentage},
                {"HasNegativeValues", s => s == "0"? (CurrentValues.CurrentValueLeft.HasNegativeValues? 1 : 0) : (CurrentValues.CurrentValueRight.HasNegativeValues? 1 : 0)},
                {"HasPositiveValues", s => s == "0"? (CurrentValues.CurrentValueLeft.HasPositiveValues? 1 : 0) : (CurrentValues.CurrentValueRight.HasPositiveValues? 1 : 0)}
            };
            return terminalDictionary;
        }
    }
}
