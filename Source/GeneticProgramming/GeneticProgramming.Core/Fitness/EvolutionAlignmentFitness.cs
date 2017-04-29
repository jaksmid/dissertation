using System.Collections.Generic;
using System.Linq;
using GeneticProgramming.Core.Metrics;
using Metadata.Distance;
using Metadata.Distance.HungarianAlgorithm;
using Metadata.Distance.Kernelization;
using Metadata.Global;
using Metadata.Prediction.Evaluation;

namespace GeneticProgramming.Core.Fitness
{
    /// <summary>
    /// Attribute alignment fitness function 
    /// </summary>
    public class EvolutionAlignmentFitness : BaseEvolutionFitness
    {
        public DummyDistanceFactory DummyDistanceFactory { get; set; }
        public EvolutionAlignmentFitness(RankingPredictorEvaluator evaluator, int numberOfNeigbors, int? pOfNorm, DummyDistanceFactory dummyDistanceFactory) : base(evaluator, new NoKernelization(), numberOfNeigbors, pOfNorm)
        {
            DummyDistanceFactory = dummyDistanceFactory;
        }

        public override DistanceMatrix CreateDistanceMatrix(List<double> coefficients, MetadataCollection metadata)
        {
            var dummyWeights = coefficients.Take(DummyDistanceFactory.WeightsNeeded).ToList();
            var dummyDistancePair = DummyDistanceFactory.CreateDummyDistances(dummyWeights);
            var restOfWeights = coefficients.Skip(DummyDistanceFactory.WeightsNeeded).ToList();
            var distanceMatrix = new DistanceMatrix(metadata, new AlignmentMetric(restOfWeights, true, P, dummyDistancePair), true);
            return distanceMatrix;
        }

        public override int GetChromosomesCount(int filterCount)
        {
            return DummyDistanceFactory.WeightsNeeded + AlignmentMetric.NumberOfCoeefficients;
        }
    }
}