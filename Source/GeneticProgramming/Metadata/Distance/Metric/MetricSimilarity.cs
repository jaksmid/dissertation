using System;
using System.Linq;
using Accord.Math;

namespace Metadata.Distance.Metric
{
    public class MetricSimilarity:IMetricSimilarity
    {
        public double ComputeMetricSimilarity(DistanceMatrix a, DistanceMatrix b)
        {
            var reshapedA = a.DistanceEntries.Reshape();
            var reshapedB = b.DistanceEntries.Reshape();
            var maxA = reshapedA.Max(entry => entry.Distance);
            var maxB = reshapedB.Max(entry => entry.Distance);
            var maxEntry=Math.Max(maxA,maxB);
            var minA = reshapedA.Min(entry => entry.Distance);
            var minB = reshapedB.Min(entry => entry.Distance);
            var minEntry=Math.Min(minA, minB);
            double nonNegativityFitness = NonNegativity(reshapedA);
            double coincidence = Coincidence(reshapedA);
            double symmetry = Symmetry(reshapedA, reshapedB, maxEntry, minEntry);
            double triangle = Triangle(a);
            return WeightErrors(nonNegativityFitness, coincidence, symmetry, triangle);
        }

        public double WeightErrors(double nonNegativity, double coincidence, double symmetry, double triangular)
        {
            return (nonNegativity+coincidence+symmetry+triangular)/4;
        }

        private double Triangle(DistanceMatrix a)
        {
            var dimSize = a.DistanceEntries.GetLength(0);
            int errors = 0;
            for (int x = 0; x < dimSize; x++)
            {
                for (int y = 0; y < dimSize; y++)
                {
                    var xy = a.DistanceEntries[x, y].Distance;
                    for (int z = 0; z < dimSize; z++)
                    {
                        var xz = a.DistanceEntries[x, z].Distance;
                        var zy= a.DistanceEntries[z, y].Distance;
                        if (xz + zy < xy)
                        {
                            errors++;
                            
                        }
                    }
                }
            }
            var totals = Math.Pow(dimSize, 3);
            return 1 - (errors/totals);
        }

        private double Symmetry(DistanceEntry[] a, DistanceEntry[] b, double max,double min)
        {
            var diff = max - min;
            if (Math.Abs(diff) < 0.0000001)
            {
                return 1;
            }
            double totalError = a.Select((t, i) => Math.Abs(t.Distance - b[i].Distance) / diff).Sum();
            totalError = totalError / a.Length;
            return 1 - totalError;
        }

        private double NonNegativity(DistanceEntry[] a)
        {
            var totalCount = a.Length;
            var nonNegative = a.Count(entry => entry.Distance >= 0);
            if (nonNegative != totalCount)
            {
                string ac = "";
            }
            return (double)nonNegative/(double)totalCount;
        }

        private double Coincidence(DistanceEntry[] a)
        {
            var totalCount = a.Length;
            var zeroCount = a.Count(entry => Math.Abs(entry.Distance) < 0.00000001 && entry.SourceName != entry.TargetName);
            return 1-(zeroCount / (double)totalCount);
        }
    }
}
