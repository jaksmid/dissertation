using System;
using System.Collections.Generic;
using System.Linq;
using GeneticProgramming.Data.Contracts;
using GeneticProgramming.Data.Statistics;

namespace GeneticProgramming.Core.Helpers
{
    /// <summary>
    /// Tools for evoloved programs
    /// </summary>
    public class ProgramHelpers
    {
        public static void FillProgramMetadataToResults(IndividualEvaluationResults result, ProgramEnvelope p)
        {
            if (p.ComputeStatistics)
            {
                var statistic = new List<ProgramStatistic>(p.Programs.Count());
                statistic.AddRange(p.Programs.Select(Evaluate));
                result.ProgramStatistics = statistic;
            }
            result.IndividualNumber = p.NumberOfIndividual;
            result.PopulationNumber = p.PopulationNumber;
            result.GenerationNumber = p.GenerationNumber;
            result.ExperimentIdentifier = p.ExperimentIdentifier;
            result.EvaluatedBy = Environment.MachineName;
            result.TaskId = p.TaskId;
        }

        /// <summary>
        /// Computes size of program
        /// </summary>
        /// <param name="p">Program to compute size</param>
        /// <returns>Porgram evaluation results with computed sizes</returns>
        public static ProgramStatistic Evaluate(Program p)
        {
            int numberOfNodes = GetNumberOfNodes(p);
            int depth = GetDepth(p);
            int width = GetWidth(p);
            
            return new ProgramStatistic { Depth = depth, NumberOfNodes = numberOfNodes, Width = width };
        }

        /// <summary>
        /// Computes number of nodes
        /// </summary>
        /// <param name="p">Input program</param>
        /// <returns>Number of nodes</returns>
        public static int GetNumberOfNodes(Program p)
        {
            if (p == null) return 0;
            int navrat = 1;
            foreach (var ch in p.Children)
            {
                navrat += GetNumberOfNodes(ch);
            }
            return navrat;
        }

        /// <summary>
        /// Computes depth
        /// </summary>
        /// <param name="p">Input program</param>
        /// <returns>Depth</returns>
        public static int GetDepth(Program p)
        {
            if (p == null) return 0;
            int navrat = 1;
            int depthbelow = 0;
            foreach (var ch in p.Children)
            {
                depthbelow = Math.Max(depthbelow, GetDepth(ch));
            }
            navrat += depthbelow;
            return navrat;
        }

        /// <summary>
        /// Computes width
        /// </summary>
        /// <param name="p">Input program</param>
        /// <returns>Width</returns>
        public static int GetWidth(Program p)
        {
            if (p == null) return 0;
            int navrat = 0;
            List<Program> patro = new List<Program> { p };
            while (patro.Count >0)
            {
                if (patro.Count > navrat) navrat = patro.Count;
                List<Program> dalsipatro = new List<Program>();
                foreach (var prog in patro)
                {
                    dalsipatro.AddRange(prog.Children);
                }
                patro = dalsipatro;
            }

            return navrat;
        }
    }
}
