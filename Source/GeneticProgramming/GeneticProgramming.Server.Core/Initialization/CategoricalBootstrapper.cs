using System.Collections.Generic;
using GeneticProgramming.Server.Core.Programs;

namespace GeneticProgramming.Server.Core.Initialization
{
    public class CategoricalBootstrapper : ManhattanDistanceBootstrapper
    {
        public CategoricalBootstrapper(ProgramTypeSet typeSet, double rescaling, List<double> weights)
        {
            TypeSet = typeSet;
            Rescaling = rescaling;
            Weights = weights;
        }

        public override string[] Labels { get; } = {
            "ChiSquareUniformDistribution",
            "TK",
            "TU",
            "CT",
            "EN",
            "PR",
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
            "SR",
            "UD",
            "MV",
            "FR"
        };
    }
}