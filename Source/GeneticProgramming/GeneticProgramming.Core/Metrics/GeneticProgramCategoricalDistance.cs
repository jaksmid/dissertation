using System;
using System.Collections.Generic;
using GeneticProgramming.Data.Contracts;
using Metadata.Attributes;
using Metadata.Distance.HungarianAlgorithm;

namespace GeneticProgramming.Core.Metrics
{
    public class GeneticProgramCategoricalDistance : GeneticProgrammingDistance<CategoricalMetadata>
    {
        public GeneticProgramCategoricalDistance(Program program, IGpDummyDistance<CategoricalMetadata> dummyDistance, bool repairToSemimetric) : base(program, dummyDistance, repairToSemimetric)
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

                //Categorical specific only
                {"UD", s => s == "0"? (CurrentValues.CurrentValueLeft.UniformDiscrete? 1 : 0) : (CurrentValues.CurrentValueRight.UniformDiscrete? 1 : 0)},
                {"TU", s => s == "0"? CurrentValues.CurrentValueLeft.RationOfDistinguishingCategoriesByUtest : CurrentValues.CurrentValueRight.RationOfDistinguishingCategoriesByUtest},
                {"TK", s => s == "0"? CurrentValues.CurrentValueLeft.RationOfDistinguishingCategoriesByKolmogorovSmirnoffSlashChiSquare : CurrentValues.CurrentValueRight.RationOfDistinguishingCategoriesByKolmogorovSmirnoffSlashChiSquare},
                {"ChiSquareUniformDistribution", s => s == "0"? CurrentValues.CurrentValueLeft.ChiSquareUniformDistribution : CurrentValues.CurrentValueRight.ChiSquareUniformDistribution}

            };
            return terminalDictionary;
        }
    }
}