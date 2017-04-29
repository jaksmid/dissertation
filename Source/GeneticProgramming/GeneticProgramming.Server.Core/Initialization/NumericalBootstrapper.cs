using System.Collections.Generic;
using GeneticProgramming.Server.Core.Programs;

namespace GeneticProgramming.Server.Core.Initialization
{
    public class NumericalBootstrapper : ManhattanDistanceBootstrapper
    {
        public NumericalBootstrapper(ProgramTypeSet typeSet, double rescaling, List<double> weights)
        {
            TypeSet = typeSet;
            Rescaling = rescaling;
            Weights = weights;
        }

        public override string[] Labels { get; } = {
            "Min",
            "Max",
            "CT",
            "EN",
            "PR",
            "KURT",
            "VAR",
            "SK",
            "Mean",
            "Distinct",
            "AverageClassCount",
            "AveragePercentageOfClass",
            "LeastFequentClassCount",
            "MedianClassCount",
            "MedianClassPercentage",
            "MissingValuesCount",
            "ModeClassCount",
            "ModeClassPercentage",
            "MostFequentClassCount",
            "NonMissingValuesCount",
            "PercentageOfLeastFrequentClass",
            "PercentageOfMissing",
            "PercentageOfMostFrequentClass",
            "PercentageOfNonMissing",
            "ValuesCount",
            "SD",
            "SR",
            "IO",
            "IU",
            "MV",
            "HasNegativeValues",
            "HasPositiveValues",
            "FR",
            "Mode",
            "Median",
            "ValueRange",
            "LowerOuterFence",
            "HigherOuterFence",
            "LowerQuartile",
            "HigherQuartile",
            "HigherConfidence",
            "LowerConfidence",
            "PositiveCount",
            "PositivePercentage",
            "NegativeCount",
            "NegativePercentage"
        };
    }
}