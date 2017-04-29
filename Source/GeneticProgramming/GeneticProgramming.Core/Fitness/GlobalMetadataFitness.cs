using System.Collections.Generic;
using GeneticProgramming.Data.Dao;
using Metadata.Distance;
using Metadata.Distance.Kernelization;
using Metadata.Global;
using Metadata.Prediction.Evaluation;

namespace GeneticProgramming.Core.Fitness
{
    /// <summary>
    /// Attribute alignment fitness function 
    /// </summary>
    public class GlobalMetadataFitness : BaseEvolutionFitness
    {
        public GlobalMetadataFitness(RankingPredictorEvaluator evaluator, IKernelization kernelization, int numberOfNeigbors, int? pOfNorm)
            : base(evaluator, kernelization, numberOfNeigbors, pOfNorm) { }

        public override DistanceMatrix CreateDistanceMatrix(List<double> coefficients, MetadataCollection metadata)
        {
            var distanceMatrix = new DistanceMatrix(metadata, new GlobalMetadataMetric(coefficients, P));
            return distanceMatrix;
        }

        public override int GetChromosomesCount(int filterCount)
        {
            return GlobalMetadataColumns.GlobalMetadataColumnsNames.Count - filterCount;
        }
    }
}