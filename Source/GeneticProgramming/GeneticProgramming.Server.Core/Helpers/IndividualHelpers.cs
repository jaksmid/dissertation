using System.Collections.Generic;
using System.Linq;
using GeneticProgramming.Data.Statistics;
using GeneticProgramming.Server.Core.GeneticProgramming;

namespace GeneticProgramming.Server.Core.Helpers
{
    /// <summary>
    /// Tools for individuals
    /// </summary>
    public static class IndividualHelpers
    {
        /// <summary>
        /// Deep copy of individual
        /// </summary>
        /// <param name="ind">Individual to copy</param>
        /// <param name="copyFitness"></param>
        /// <returns>Copy</returns>
        public static Individual CopyIndividual(this Individual ind, bool copyFitness =true)
        {
            var i = new Individual
            {
                PopulationNumber = ind.PopulationNumber,
                NumberOfIndividual = ind.NumberOfIndividual,
                GenerationNumber = ind.GenerationNumber,
                Programs = ind.Programs.Select(CopyGpProgram).ToList(),
                Rank = ind.Rank,
                CrowdingDistance = ind.CrowdingDistance
            };
            if (!copyFitness) return i;
            if (ind.MultiObjectiveFitness != null)
            {
                i.MultiObjectiveFitness=new List<double>();
                foreach (var d in ind.MultiObjectiveFitness)
                {
                    i.MultiObjectiveFitness.Add(d);
                }
            }
            if (ind.MultiObjectiveValidation != null)
            {
                i.MultiObjectiveValidation = new List<double>();
                foreach (var d in ind.MultiObjectiveValidation)
                {
                    i.MultiObjectiveValidation.Add(d);
                }
            }
            return i;
        }

        public static ProgramStatistic CopyStatistic(this ProgramStatistic st)
        {
            var newStatistic = new ProgramStatistic
            {
                Depth = st.Depth,
                NumberOfNodes = st.NumberOfNodes,
                Width = st.Width
            };
            return newStatistic;
        }

        public static GpProgram CopyGpProgram(GpProgram pr)
        {
            var p = new GpProgram
            {
                OperatorInstance = CopyOperator(pr.OperatorInstance,null),
                Statistic = CopyStatistic(pr.Statistic)
            };
            return p;
        }

        /// <summary>
        /// Deep copy of operator instance
        /// </summary>
        /// <param name="op">Operator to copy</param>
        /// <param name="parent">Copied parent</param>
        /// <returns>Operator copy</returns>
        public static Operator CopyOperator(Operator op, Operator parent)
        {
            var oper = new Operator
            {
                Parent = parent,
                BaseOperator = op.BaseOperator,
                Value = op.Value,
                Children = new List<Operator>()
            };
            foreach (var v in op.Children)
            {
                oper.Children.Add(CopyOperator(v, oper));
            }
            return oper;
        }

        /// <summary>
        /// Nulls individual evaluation
        /// </summary>
        /// <param name="ind"></param>
        public static void NullEvaluation(Individual ind)
        {
            ind.MultiObjectiveFitness = null;
            ind.MultiObjectiveValidation = null;
            ind.CrowdingDistance = null;
            ind.Rank = null;
            foreach (var gpProgram in ind.Programs)
            {
                gpProgram.Statistic = null;
            }
        }
    }
}