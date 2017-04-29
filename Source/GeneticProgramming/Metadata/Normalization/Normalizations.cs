using System;
using System.Collections.Generic;
using System.Linq;

namespace Metadata.Normalization
{
    public class Normalizations
    {
        public static Func<float, float> CreateTransformationFunction(List<float> values)
        {
            float min = values.Min();
            float max = values.Max();
            float diff = max - min;
            if (diff < 0.0000001)
            {
                return x => 0;
            }
            return x => (x - min) / diff;
        }

        public static Func<double, double> CreateTransformationFunction(List<double> values)
        {
            double min = values.Min();
            double max = values.Max();
            double diff = max - min;
            if (diff < 0.0000001)
            {
                return x => 0;
            }
            return x => (x - min) / diff;
        }

        public static Func<double, double> CreateLogTransformationFunction(List<double> values)
        {
            double min = values.Min();
            double max = values.Max();
            var shift = 0d;
            if (min < 1)
            {
                shift = 1 - min;
                min += shift;
                max += shift;
            }
            min = Math.Log(min);
            max = Math.Log(max);
            double diff = max - min;
            if (diff < 0.0000001)
            {
                return x => 0;
            }
            diff = max - min;
            return x => (Math.Log(x+shift) - min) / diff;
        }

        public static Func<double, double> CreateMinMaxNormalizationWithSigmoidOutliers(List<double> values)
        {
            values.Sort();
            var valuesCounts = values.Count;
            var lowerPercentile = (int) (0.05*valuesCounts);
            var leftBound = values[lowerPercentile];
            var upperPercentile = (int) (0.9*valuesCounts);
            var rightBound = values[upperPercentile];
            var diff = rightBound - leftBound;
            var lowerSteeptnessFactor = leftBound - values[(int)(0.025 * valuesCounts)];
            var upperSteepnessFactor =values[(int)(0.925 * valuesCounts)] - rightBound;
            return x => x >= leftBound && x <= rightBound ? 0.6 * (x - leftBound) / diff+0.2: x < leftBound? CalculateSigmoid(x + 2*lowerSteeptnessFactor,0,0.2, 1/lowerSteeptnessFactor) : CalculateSigmoid(x-2*upperSteepnessFactor, 0.8, 1, 1/upperSteepnessFactor);
        }

        public static double CalculateSigmoid(double value, double from, double to, double steepness)
        {
            if (value > 1)
            {
                var f = 1;
            }
            var halfValue = value/2;
            var tanhValueHalf = Math.Tanh(halfValue* steepness);
            var diff = to - from;
            var sigmoid = (1 + tanhValueHalf)/2;
            var result = sigmoid*diff + from;
            return result;
        }

        public static Func<double, double> CreateUniformTransformation(List<double> values)
        {
            var unique = values.Distinct().ToList();
            unique.Sort();
            var dictionary = new Dictionary<double, double>();
            double increment = 1d/(unique.Count-1);
            for (int i = 0; i < unique.Count; i++)
            {
                dictionary.Add(unique[i],i*increment);
            }
            return x => dictionary[x];
        }
    }
}
