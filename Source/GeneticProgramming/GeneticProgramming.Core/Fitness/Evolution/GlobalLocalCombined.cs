using System.Collections.Generic;
using System.Linq;
using GeneticProgramming.Core.Metrics;
using GeneticProgramming.Data.Dao;
using Metadata.Distance;
using Metadata.Distance.HungarianAlgorithm;
using Metadata.Distance.Kernelization;
using Metadata.Global;
using Metadata.Prediction.Evaluation;

namespace GeneticProgramming.Core.Fitness.Evolution
{
    public class GlobalLocalCombined: BaseEvolutionFitness
    {
        public DummyDistanceFactory DummyDistanceFactory { get; set; }

        public GlobalLocalCombined(RankingPredictorEvaluator evaluator, IKernelization kernelization, int numberOfNeigbors, int? pOfNorm, DummyDistanceFactory dummyDistanceFactory) : base(evaluator, kernelization, numberOfNeigbors, pOfNorm)
        {
            DummyDistanceFactory = dummyDistanceFactory;
        }

        public override DistanceMatrix CreateDistanceMatrix(List<double> coefficients, MetadataCollection metadata )
        {
            var dummyWeights = coefficients.Take(DummyDistanceFactory.WeightsNeeded).ToList();
            var dummyDistancePair = DummyDistanceFactory.CreateDummyDistances(dummyWeights);
            var restOfWeights = coefficients.Skip(DummyDistanceFactory.WeightsNeeded).ToList();
            var metrics = new List<IMetadataMetric>
            {
                //new MasterThesisMetric(),
                new AlignmentMetric(restOfWeights.GetRange(2, AlignmentMetric.NumberOfCoeefficients), true, P, dummyDistancePair),
                new GlobalMetadataMetric(restOfWeights.GetRange(2+AlignmentMetric.NumberOfCoeefficients,restOfWeights.Count-2-AlignmentMetric.NumberOfCoeefficients), P)
            };
            var distanceMatrix = new DistanceMatrix(metadata, new MetadataMetricCombiner(metrics, restOfWeights.GetRange(0,2)));
            return distanceMatrix;
        }

        public override int GetChromosomesCount(int filterCount)
        {
            return GlobalMetadataColumns.GlobalMetadataColumnsNames.Count - filterCount+2+AlignmentMetric.NumberOfCoeefficients;
        }
    }
}
