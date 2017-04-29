using System;
using System.Collections.Generic;
using GeneticProgramming.Core.Metrics;
using GeneticProgramming.Data.Contracts;
using log4net;
using Metadata.Distance;
using Metadata.Distance.HungarianAlgorithm;
using Metadata.Distance.Kernelization;
using Metadata.Distance.Metric;
using Metadata.Global;
using Metadata.Prediction;
using Metadata.Prediction.Evaluation;

namespace GeneticProgramming.Core.Fitness
{
    /// <summary>
    /// Attribute alignment fitness function 
    /// </summary>
    public class AttributeAlignmentFitness : IFitness
    {
        public DummyDistanceFactory DummyDistanceFactory { get; set; }
        private static readonly ILog Logger = LogManager.GetLogger(typeof(AttributeAlignmentFitness));
        public IKernelization Kernelization { get; set; }
        public int NumberOfNeigbours { get; set; }
        public bool RepairToSemimetric { get; set; }
        public bool RepairToAttributeSemimetric { get; set; }
        public bool IncludeMetricSimilarity { get; set; }
        public bool IncludeAttributeMetricSimilarity { get; set; }
        public RankingPredictorEvaluator Evaluator { get; set; }
        public MetricSimilarity SimilarityEvaluator { get; set; }
        


        public AttributeAlignmentFitness(RankingPredictorEvaluator evaluator, IKernelization kernelization, DummyDistanceFactory dummyDistanceFactory, int numberOfNeigbours, bool repairToSemimetric, bool repairToAttributeSemimetric,
            bool includeMetricSimilarity= true, bool includeAttributeMetricSimilarity = false)
        {
            Kernelization = kernelization;
            DummyDistanceFactory = dummyDistanceFactory;
            NumberOfNeigbours = numberOfNeigbours;
            RepairToSemimetric = repairToSemimetric;
            RepairToAttributeSemimetric = repairToAttributeSemimetric;
            IncludeMetricSimilarity = includeMetricSimilarity;
            IncludeAttributeMetricSimilarity = includeAttributeMetricSimilarity;
            Evaluator = evaluator;
            if (includeMetricSimilarity)
            {
                SimilarityEvaluator = new MetricSimilarity();
            }
            DummyDistancePair = DummyDistanceFactory.CreateGpDummyDistances();
        }

        public GpDummyDistancePair DummyDistancePair { get; set; }

        public Func<MetadataCollection, IRankingPredictor> CreatePredictorFunction(DistanceMatrixResult result, Program a, Program b = null)
        {
            return metadata=>CreatePredictor(metadata, result, a, b);
        }

        public IRankingPredictor CreatePredictor(MetadataCollection metadata, DistanceMatrixResult result, Program a, Program b = null)
        {
            var distanceMatrix = new DistanceMatrix(metadata, new AlignmentMetric(a, b,DummyDistancePair, RepairToSemimetric, RepairToAttributeSemimetric));
            Kernelization.FixMatrix(distanceMatrix);
            result.Result = distanceMatrix;
            result.Metadata = metadata;
            var threeNn = new KnnPredictor(NumberOfNeigbours, Evaluator.Provider, distanceMatrix);
            return threeNn;
        }

        private double EvaluateMetricSimmilarity(DistanceMatrixResult firstFitnessResult, Program p, Program p2)
        {
            var distanceMatrixB = new DistanceMatrix(firstFitnessResult.Metadata, new MetricArgumentSwitcher(new AlignmentMetric(p, p2, DummyDistancePair, RepairToSemimetric, RepairToAttributeSemimetric)));
            var metricSimilarity = SimilarityEvaluator.ComputeMetricSimilarity(firstFitnessResult.Result, distanceMatrixB);
            return metricSimilarity;
        }

        private List<Double> Evaluate(Program p1, Program p2, bool validation)
        {
            var result = new List<double>();
            //Will be filled later in the Evaluator, see CreatePredictor method 
            var distanceResult = new DistanceMatrixResult();
            double fitness1;
            if (validation)
            {
                fitness1 = Evaluator.GetValidationResults(CreatePredictorFunction(distanceResult, p1, p2));
            }
            else
            {
                fitness1 = Evaluator.GetTrainingResults(CreatePredictorFunction(distanceResult, p1, p2));
            }
            result.Add(fitness1);
            if (IncludeMetricSimilarity)
            {
                var fitness2 = EvaluateMetricSimmilarity(distanceResult, p1, p2);
                result.Add(fitness2);
            }
            else if (IncludeAttributeMetricSimilarity)
            {
                double fitness2;
                AttributeMetricSimilarity attributeMetricSimilarity = new AttributeMetricSimilarity(p1, p2, RepairToAttributeSemimetric, DummyDistancePair);
                if (validation)
                {
                    fitness2 = attributeMetricSimilarity.ComputeAttributeMetricSimilarity(Evaluator.ValidationMetadata.Metadatas);
                }
                else
                {
                    fitness2 = attributeMetricSimilarity.ComputeAttributeMetricSimilarity(Evaluator.TrainingMetadata.Metadatas);
                }
                result.Add(fitness2);
            }
            Logger.Info(string.Format("Evaluated individual: {0}. Validation: {1}",fitness1, validation));
            return result;
        }

        public List<Double> Evaluate(Program[] programs)
        {
            return Evaluate(programs[0], programs[1], false);
        }

        public List<Double> Validate(Program[] programs)
        {
            return Evaluate(programs[0], programs[1], true);
        }
    }
}