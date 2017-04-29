using System;
using System.Collections.Generic;
using GeneticProgramming.Server.Core.GeneticProgramming;

namespace GeneticProgramming.Server.Core.GeneticOperators.Comparers
{
    public class AntiBloatFitnessComparer : IComparer<Individual>
    {
        public int DepthToPenalize { get; set; }
        public int WidthToPenalize { get; set; }
        public int NodesToPenalize { get; set; }
        public int DepthPenalization { get; set; }
        public int WidthPenalization { get; set; }
        public int NodesPenalization { get; set; }

        public AntiBloatFitnessComparer(int depthToPenalize, int widthToPenalize, int nodesToPenalize,
            int depthPenalization, int widthPenalization, int nodesPenalization)
        {
            DepthToPenalize = depthToPenalize;
            WidthToPenalize = widthToPenalize;
            NodesToPenalize = nodesToPenalize;
            DepthPenalization = depthPenalization;
            WidthPenalization = widthPenalization;
            NodesPenalization = nodesPenalization;
        }

        public double GetPenalization(double actual, int maxWanted, int percantageCorresponsTo)
        {
            var diff = actual - maxWanted;
            double percenatgesDown = diff/percantageCorresponsTo;
            return Math.Max(0.9, 1 - (percenatgesDown*0.01));
        }

        public double GetAmendedFitness(Individual i)
        {
            if (i.Fitness == null)
            {
                return 0;
            }
            var result = i.Fitness.Value;
            var statistics = i.Statistics;
            foreach (var programStatistic in statistics)
            {
                if (programStatistic.Depth > DepthToPenalize)
                {
                    result = result*GetPenalization(programStatistic.Depth.Value, DepthToPenalize, DepthPenalization);
                }
                if (programStatistic.Width > WidthToPenalize)
                {
                    result = result * GetPenalization(programStatistic.Width.Value, WidthToPenalize, WidthPenalization);
                }
                if (programStatistic.NumberOfNodes > NodesToPenalize)
                {
                    result = result * GetPenalization(programStatistic.NumberOfNodes.Value, NodesToPenalize, NodesPenalization);
                }
            }
            return result;
        }

        public int Compare(Individual x, Individual y)
        {
            if (x.Fitness == null || y.Fitness == null) return 0;
            double xFitness = GetAmendedFitness(x);
            double yFitness = GetAmendedFitness(y);
            return xFitness.CompareTo(yFitness);
        }
    }
}