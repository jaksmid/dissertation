using System;
using System.Collections.Generic;
using System.Linq;

namespace Metadata.Distance.HungarianAlgorithm
{
    public class WeightedPNormFloatDistance
    {
        public List<double> Weights { get; set; }
        public int? P { get; set; }

        public WeightedPNormFloatDistance(List<double> weights, int? p)
        {
            Weights = weights;
            P = p;
        }

        public double MeasureDistance(List<float> left, List<float> right)
        {
            if (P == 1)
            {
                return Weights.Select((t, i) => t * Math.Abs(left[i] - right[i])).Sum();
            }
            if (P != null)
            {
                var p = (double)P;
                return Math.Pow(Weights.Select((t, i) => t * Math.Pow(left[i] - right[i], p)).Sum(), 1 / p);
            }
            return Weights.Select((t, i) => t * Math.Abs(left[i] - right[i])).Max();
        }
    }
}