using System;
using System.Collections.Generic;
using System.Linq;
using Accord.Genetic;
using log4net;
using Metadata.Distance;
using Metadata.Distance.Kernelization;
using Metadata.Global;
using Metadata.Prediction;
using Metadata.Prediction.Evaluation;

namespace GeneticProgramming.Core.Fitness
{
    /// <summary>
    /// Attribute alignment fitness function 
    /// </summary>
    public abstract class BaseEvolutionFitness : IFitnessFunction
    {
        public IKernelization Kernelization { get; set; }
        public int NumberOfNeigbors { get; set; }
        public RankingPredictorEvaluator Evaluator { get; set; }
        public int? P { get; set; }

        protected static readonly ILog Logger = LogManager.GetLogger(typeof(BaseEvolutionFitness));

        public CoefficientsCache EvaluatedCache = new CoefficientsCache();
        public CoefficientsCache ValidatedCache = new CoefficientsCache();

        protected BaseEvolutionFitness(RankingPredictorEvaluator evaluator, IKernelization kernelization, int numberOfNeigbors, int? pOfNorm)
        {
            Kernelization = kernelization;
            NumberOfNeigbors = numberOfNeigbors;
            Evaluator = evaluator;
            P = pOfNorm;
        }

        public Func<MetadataCollection, IRankingPredictor> CreatePredictorFunction(List<Double> coefficients)
        {
            return metadata=>CreatePredictor(coefficients, metadata);
        }

        public IRankingPredictor CreatePredictor(List<Double> coefficients, MetadataCollection metadata)
        {
            var distanceMatrix = CreateDistanceMatrix(coefficients, metadata);
            Kernelization.FixMatrix(distanceMatrix);
            var knn = new KnnPredictor(NumberOfNeigbors, Evaluator.Provider, distanceMatrix);
            return knn;
        }

        public abstract DistanceMatrix CreateDistanceMatrix(List<double> coefficients, MetadataCollection metadata);
        
        
        public double Evaluate(IChromosome x)
        {
            var chrom = (DoubleArrayChromosome)x;
            var coefficients = chrom.Value.Select(Math.Abs).ToList();
            var cacheResult = EvaluatedCache.GetValueFromCache(coefficients);
            if (cacheResult != null)
            {
                Logger.Info("Evaluated from cache: fitness: " + cacheResult.Value);
                return cacheResult.Value;
            }
            var result = Evaluator.GetTrainingResults(CreatePredictorFunction(coefficients));
            Logger.Info("Evaluated individual: fitness: " + result);
            EvaluatedCache.AddValueToCache(coefficients, result);
            return result;
        }

        public double Validate(IChromosome x)
        {
            var chrom = (DoubleArrayChromosome)x;
            var coefficients = chrom.Value.Select(Math.Abs).ToList();
            var cacheResult = ValidatedCache.GetValueFromCache(coefficients);
            if (cacheResult != null)
            {
                Logger.Info("Validated from cache: fitness: " + cacheResult.Value);
                return cacheResult.Value;
            }
            var result = Evaluator.GetValidationResults(CreatePredictorFunction(coefficients));
            Logger.Info("Validated individual: fitness: " + result);
            ValidatedCache.AddValueToCache(coefficients, result);
            return result;
        }

        public abstract int GetChromosomesCount(int filterCount);
    }

    public class CoefficientsCache
    {
        public Dictionary<double, List<CacheResult>> SumToListOfCoeeficients = new Dictionary<double, List<CacheResult>>();

        public double? GetValueFromCache(List<double> coefficients)
        {
            double sum = coefficients.Sum();
            if (SumToListOfCoeeficients.ContainsKey(sum))
            {
                var result = SumToListOfCoeeficients[sum].FirstOrDefault(x => x.Coefficients.SequenceEqual(coefficients));
                if (result != null)
                {
                    return result.Value;
                }
            }
            return null;
        }

        public void AddValueToCache(List<double> coefficients, double computedValue)
        {
            double sum = coefficients.Sum();
            if (!SumToListOfCoeeficients.ContainsKey(sum))
            {
                SumToListOfCoeeficients.Add(sum, new List<CacheResult>());
            }
            SumToListOfCoeeficients[sum].Add(new CacheResult(coefficients, computedValue));
        }
    }

    public class CacheResult
    {
        public List<double> Coefficients { get; set; }
        public double Value { get; set; }

        public CacheResult(List<double> coefficients, double value)
        {
            Coefficients = coefficients;
            Value = value;
        }
    }
}