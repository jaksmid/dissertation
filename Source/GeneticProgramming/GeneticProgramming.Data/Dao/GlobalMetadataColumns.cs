using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;

namespace GeneticProgramming.Data.Dao
{
    public class GlobalMetadataColumns
    {
        public static List<string> GetFilteredColumns(HashSet<string> filter)
        {
            return GlobalMetadataColumnsNames.Where(col => !filter.Contains(col)).ToList();
        }

        public static HashSet<string> CreateFilter(List<Tuple<string, string>> rangeFilter)
        {
            var toReturn = new HashSet<string>();
            var nameToIndex = new Dictionary<string, int>();
            GlobalMetadataColumnsNames.ForEach((column, index) => nameToIndex.Add(column, index));
            foreach (var tuple in rangeFilter)
            {
                int from = nameToIndex[tuple.Item1];
                int to = nameToIndex[tuple.Item2];
                for (int i = from; i <= to; i++)
                {
                    string filterColumnName = GlobalMetadataColumnsNames[i];
                    if (!toReturn.Contains(filterColumnName))
                    {
                        toReturn.Add(filterColumnName);
                    }
                }
            }
            return toReturn;
        } 

        public static List<string> GlobalMetadataColumnsNames = new List<string>
        {
            "ClassCount",
            "ClassEntropy",
            "DecisionStumpAUC",
            "DecisionStumpErrRate",
            "DecisionStumpKappa",
            "DefaultAccuracy",
            "Dimensionality",
            "EquivalentNumberOfAtts",
            "HoeffdingAdwin_changes",
            "HoeffdingAdwin_warnings",
            "HoeffdingDDM_changes",
            "HoeffdingDDM_warnings",
            "IncompleteInstanceCount",
            "InstanceCount",
            "J48_00001_AUC",
            "J48_00001_ErrRate",
            "J48_00001_Kappa",
            "J48_0001_AUC",
            "J48_0001_ErrRate",
            "J48_0001_Kappa",
            "J48_001_AUC",
            "J48_001_ErrRate",
            "J48_001_Kappa",
            "JRipAUC",
            "JRipErrRate",
            "JRipKappa",
            "MajorityClassSize",
            "MaxNominalAttDistinctValues",
            "MeanAttributeEntropy",
            "MeanKurtosisOfNumericAtts",
            "MeanMeansOfNumericAtts",
            "MeanMutualInformation",
            "MeanNominalAttDistinctValues",
            "MeanSkewnessOfNumericAtts",
            "MeanStdDevOfNumericAtts",
            "MinNominalAttDistinctValues",
            "MinorityClassSize",
            "NBTreeAUC",
            "NBTreeErrRate",
            "NBTreeKappa",
            "NaiveBayesAUC",
            "NaiveBayesAdwin_changes",
            "NaiveBayesAdwin_warnings",
            "NaiveBayesDdm_changes",
            "NaiveBayesDdm_warnings",
            "NaiveBayesErrRate",
            "NaiveBayesKappa",
            "NegativePercentage",
            "NoiseToSignalRatio",
            "NumAttributes",
            "NumBinaryAtts",
            "NumMissingValues",
            "NumNominalAtts",
            "NumNumericAtts",
            "NumberOfClasses",
            "NumberOfFeatures",
            "NumberOfInstances",
            "NumberOfInstancesWithMissingValues",
            "NumberOfMissingValues",
            "NumberOfNumericFeatures",
            "NumberOfSymbolicFeatures",
            "PercentageOfBinaryAtts",
            "PercentageOfMissingValues",
            "PercentageOfNominalAtts",
            "PercentageOfNumericAtts",
            "PositivePercentage",
            "REPTreeDepth1AUC",
            "REPTreeDepth1ErrRate",
            "REPTreeDepth1Kappa",
            "REPTreeDepth2AUC",
            "REPTreeDepth2ErrRate",
            "REPTreeDepth2Kappa",
            "REPTreeDepth3AUC",
            "REPTreeDepth3ErrRate",
            "REPTreeDepth3Kappa",
            "RandomTreeDepth1AUC",
            "RandomTreeDepth1ErrRate",
            "RandomTreeDepth1Kappa",
            "RandomTreeDepth2AUC",
            "RandomTreeDepth2ErrRate",
            "RandomTreeDepth2Kappa",
            "RandomTreeDepth3AUC",
            "RandomTreeDepth3ErrRate",
            "RandomTreeDepth3Kappa",
            "SVMe1AUC",
            "SVMe1ErrRate",
            "SVMe1Kappa",
            "SVMe2AUC",
            "SVMe2ErrRate",
            "SVMe2Kappa",
            "SVMe3AUC",
            "SVMe3ErrRate",
            "SVMe3Kappa",
            "SimpleLogisticAUC",
            "SimpleLogisticErrRate",
            "SimpleLogisticKappa",
            "StdvNominalAttDistinctValues",
            "kNN_1NAUC",
            "kNN_1NErrRate",
            "kNN_1NKappa",
            "kNN_2NAUC",
            "kNN_2NErrRate",
            "kNN_2NKappa",
            "kNN_3NAUC",
            "kNN_3NErrRate",
            "kNN_3NKappa"
        };
    }
}
