using System;
using System.Collections.Generic;
using System.Linq;

namespace Metadata.Distance.HungarianAlgorithm
{
    public class WeightedPNormDistance<T>
    {
        public List<double> Weights { get; set; }
        public int? P { get; set; }

        public WeightedPNormDistance(List<double> weights, int? p)
        {
            Weights = weights;
            P = p;
        }

        public double MeasureDistance(List<Func<T, double>> selectors, T left, T right)
        {
            if (P == 1)
            {
                return selectors.Select((t, i) => Weights[i]*Math.Abs(t(left) - t(right))).Sum();
            }
            if (P != null)
            {
                var p = (double) P;
                return Math.Pow(selectors.Select((t, i) => Weights[i] * Math.Pow(t(left) - t(right),p)).Sum(),1/p);
            }
            return selectors.Select((t, i) => Weights[i] * Math.Abs(t(left) - t(right))).Max();
        }
    }
}