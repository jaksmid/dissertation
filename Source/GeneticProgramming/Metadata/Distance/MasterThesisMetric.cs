using System;
using System.Collections.Generic;
using System.Linq;
using Metadata.Global;

namespace Metadata.Distance
{
    public class MasterThesisMetric : IMetadataMetric
    {
        public double MeasureDistance(DatasetMetadata datasetA, DatasetMetadata datasetB)
        {
            double maxerrorCat;
            double maxerrorInt;
            double navrat = GetCategoricalDistance(datasetA, datasetB, out maxerrorCat);
            navrat += GetIntegerDistance(datasetA, datasetB, out maxerrorInt);
            double maxError = Math.Max(maxerrorCat, maxerrorInt);
            navrat += GetRealDistance(datasetA, datasetB, maxError);
            return navrat;
        }

        private double MeasureAlignedDistance(List<Double> listA, List<Double> listB, out double maxError )
        {
            maxError = 0;
            double navrat = 0;
            listA.Sort();
            listA.Reverse();
            listB.Sort();
            listB.Reverse();
            for (int i = 0; i < Math.Min(listB.Count, listA.Count); i++)
            {
                double error=Math.Abs(listA[i] - listB[i]);
                if (error > maxError) maxError = error;
                navrat += error;
            }
            if (listA.Count != listB.Count)
            {
                for (int i = Math.Min(listB.Count, listA.Count); i < Math.Max(listB.Count, listA.Count); i++)
                {
                    if (listA.Count > listB.Count)
                    {
                        double error = listA[i];
                        if (error > maxError) maxError = error;
                        navrat += listA[i];
                    }
                    else
                    {
                        double error = listB[i];
                        if (error > maxError) maxError = error;
                        navrat += listB[i];
                    }
                }
            }
            return navrat;
        }

        public Double GetRealDistance(DatasetMetadata m1, DatasetMetadata m2, double maxError)
        {
            int real1=m1.NumberOfRealAttributes;
            int real2=m2.NumberOfRealAttributes;
            return Math.Abs(real2 - real1)*8; //Math.Max(4, maxError);
        }

        public Double GetCategoricalDistance(DatasetMetadata m1, DatasetMetadata m2, out double maxError)
        {
            var categories1 = m1.CategoricalAttributes.Select(v => v.Distinct).ToList();
            var categories2 = m2.CategoricalAttributes.Select(v => v.Distinct).ToList();
            return MeasureAlignedDistance(categories1, categories2, out maxError);
        }

        private Double GetIntegerDistance(DatasetMetadata m1, DatasetMetadata m2, out double maxError)
        {
            var categories1 = m1.IntegerAttributes.Select(v => v.Max - v.Min).ToList();
            var categories2 = m2.IntegerAttributes.Select(v => v.Max - v.Min).ToList();
            return MeasureAlignedDistance(categories1, categories2, out maxError);
        }
    }
}