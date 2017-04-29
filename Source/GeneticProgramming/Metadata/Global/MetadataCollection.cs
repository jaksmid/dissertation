using System;
using System.Collections.Generic;
using System.Linq;
using Metadata.Attributes;
using Metadata.Normalization;

namespace Metadata.Global
{
    public class MetadataCollection
    {
        public DatasetMetadata GetDatasetMetadata(string datasetName)
        {
            return Metadatas.First(m => m.Name == datasetName);
        }

        public MetadataCollection()
        {
            
        }

        public MetadataCollection(IEnumerable<DatasetMetadata> metadatas, bool normalize = false)
        {
            Metadatas = metadatas.ToList();
            if (normalize)
            {
                Func<List<double>, Func<double, double>> transformationFunction = Normalizations.CreateTransformationFunction;
                Func<List<double>, Func<double, double>> secondTransformationFunction = Normalizations.CreateTransformationFunction;
                TransformNumericalAttribute(x => x.Max, (x, y) => x.Max = y, transformationFunction);
                TransformNumericalAttribute(x=>x.Min, (x,y) => x.Min = y, transformationFunction);
                TransformNumericalAttribute(x => x.Kurtosis, (x, y) => x.Kurtosis = y, transformationFunction);
                TransformNumericalAttribute(x => x.Mean, (x, y) => x.Mean = y, transformationFunction);
                TransformNumericalAttribute(x => x.Skewness, (x, y) => x.Skewness = y, transformationFunction);
                TransformNumericalAttribute(x => x.StandardDeviation, (x, y) => x.StandardDeviation = y, transformationFunction);
                TransformNumericalAttribute(x => x.Variance, (x, y) => x.Variance = y, transformationFunction);
                TransformNumericalAttribute(x => x.Mode, (x, y) => x.Mode = y, transformationFunction);
                TransformNumericalAttribute(x => x.Median, (x, y) => x.Median = y, transformationFunction);
                TransformNumericalAttribute(x => x.ValueRange, (x, y) => x.ValueRange = y, transformationFunction);
                TransformNumericalAttribute(x => x.LowerOuterFence, (x, y) => x.LowerOuterFence = y, transformationFunction);
                TransformNumericalAttribute(x => x.HigherOuterFence, (x, y) => x.HigherOuterFence = y, transformationFunction);
                TransformNumericalAttribute(x => x.HigherQuartile, (x, y) => x.HigherQuartile = y, transformationFunction);
                TransformNumericalAttribute(x => x.LowerQuartile, (x, y) => x.LowerQuartile = y, transformationFunction);
                TransformNumericalAttribute(x => x.HigherConfidence, (x, y) => x.HigherConfidence = y, transformationFunction);
                TransformNumericalAttribute(x => x.LowerConfidence, (x, y) => x.LowerConfidence = y, transformationFunction);
                TransformNumericalAttribute(x => x.PositiveCount, (x, y) => x.PositiveCount = y, transformationFunction);
                TransformNumericalAttribute(x => x.NegativeCount, (x, y) => x.NegativeCount = y, transformationFunction);

                TransformNumericalAttribute(x => x.Distinct, (x, y) => x.Distinct = y, transformationFunction);
                TransformNumericalAttribute(x => x.PearsonCorrellationCoefficient, (x, y) => x.PearsonCorrellationCoefficient = y, secondTransformationFunction);
                TransformNumericalAttribute(x => x.SpearmanCorrelationCoefficient, (x, y) => x.SpearmanCorrelationCoefficient = y, secondTransformationFunction);
                TransformNumericalAttribute(x => x.CovarianceWithTarget, (x, y) => x.CovarianceWithTarget = y, transformationFunction);
                TransformNumericalAttribute(x => x.Entropy, (x, y) => x.Entropy = y, secondTransformationFunction);
                TransformNumericalAttribute(x => x.ValuesCount, (x, y) => x.ValuesCount = y, transformationFunction);
                TransformNumericalAttribute(x => x.NonMissingValuesCount, (x, y) => x.NonMissingValuesCount = y, transformationFunction);
                TransformNumericalAttribute(x => x.MissingValuesCount, (x, y) => x.MissingValuesCount = y, transformationFunction);
                TransformNumericalAttribute(x => x.AverageClassCount, (x, y) => x.AverageClassCount = y, transformationFunction);
                TransformNumericalAttribute(x => x.MostFequentClassCount, (x, y) => x.MostFequentClassCount = y, transformationFunction);
                TransformNumericalAttribute(x => x.LeastFequentClassCount, (x, y) => x.LeastFequentClassCount = y, transformationFunction);
                TransformNumericalAttribute(x => x.ModeClassCount, (x, y) => x.ModeClassCount = y, transformationFunction);
                TransformNumericalAttribute(x => x.MedianClassCount, (x, y) => x.MedianClassCount = y, transformationFunction);

                TransformCategoricalAttribute(x => x.Distinct, (x, y) => x.Distinct = y, transformationFunction);
                TransformCategoricalAttribute(x => x.PearsonCorrellationCoefficient, (x, y) => x.PearsonCorrellationCoefficient = y, secondTransformationFunction);
                TransformCategoricalAttribute(x => x.SpearmanCorrelationCoefficient, (x, y) => x.SpearmanCorrelationCoefficient = y, secondTransformationFunction);
                TransformCategoricalAttribute(x => x.CovarianceWithTarget, (x, y) => x.CovarianceWithTarget = y, transformationFunction);
                TransformCategoricalAttribute(x => x.Entropy, (x, y) => x.Entropy = y, secondTransformationFunction);
                TransformCategoricalAttribute(x => x.ValuesCount, (x, y) => x.ValuesCount = y, transformationFunction);
                TransformCategoricalAttribute(x => x.NonMissingValuesCount, (x, y) => x.NonMissingValuesCount = y, transformationFunction);
                TransformCategoricalAttribute(x => x.MissingValuesCount, (x, y) => x.MissingValuesCount = y, transformationFunction);
                TransformCategoricalAttribute(x => x.AverageClassCount, (x, y) => x.AverageClassCount = y, transformationFunction);
                TransformCategoricalAttribute(x => x.MostFequentClassCount, (x, y) => x.MostFequentClassCount = y, transformationFunction);
                TransformCategoricalAttribute(x => x.LeastFequentClassCount, (x, y) => x.LeastFequentClassCount = y, transformationFunction);
                TransformCategoricalAttribute(x => x.ModeClassCount, (x, y) => x.ModeClassCount = y, transformationFunction);
                TransformCategoricalAttribute(x => x.MedianClassCount, (x, y) => x.MedianClassCount = y, transformationFunction);

                TransformCategoricalAttribute(x => x.ChiSquareUniformDistribution, (x, y) => x.ChiSquareUniformDistribution = y, secondTransformationFunction);
                TransformCategoricalAttribute(x => x.RationOfDistinguishingCategoriesByKolmogorovSmirnoffSlashChiSquare, (x, y) => x.RationOfDistinguishingCategoriesByKolmogorovSmirnoffSlashChiSquare = y, secondTransformationFunction);
                TransformCategoricalAttribute(x => x.RationOfDistinguishingCategoriesByUtest, (x, y) => x.RationOfDistinguishingCategoriesByUtest = y, secondTransformationFunction);
            }
        }

        public void TransformCategoricalAttribute(Func<CategoricalMetadata, double> getFunction, Action<CategoricalMetadata, double> setAction, 
            Func<List<double>, Func<double, double>> createTransformation)
        {
            var values = new List<double>();
            foreach (var datasetMetadata in Metadatas)
            {
                values.AddRange(datasetMetadata.CategoricalAttributes.Select(getFunction));
            }
            var transformation = createTransformation(values);
            foreach (var datasetMetadata in Metadatas)
            {
                foreach (var categoricalMetadata in datasetMetadata.CategoricalAttributes)
                {
                    setAction(categoricalMetadata, transformation(getFunction(categoricalMetadata)));
                }
            }
        }

        public void TransformNumericalAttribute(Func<NumericalAttribute, double> getFunction,
            Action<NumericalAttribute, double> setAction,
            Func<List<double>, Func<double, double>> createTransformation)
        {
            var values = new List<double>();
            foreach (var datasetMetadata in Metadatas)
            {
                values.AddRange(datasetMetadata.NumericalAttributes.Select(getFunction));
            }
            var transformation = createTransformation(values);
            foreach (var datasetMetadata in Metadatas)
            {
                foreach (var numericalAttribute in datasetMetadata.NumericalAttributes)
                {
                    setAction(numericalAttribute, transformation(getFunction(numericalAttribute)));
                }
            }
            values = new List<double>();
            foreach (var datasetMetadata in Metadatas)
            {
                values.AddRange(datasetMetadata.NumericalAttributes.Select(getFunction));
            }
            values.Sort();
        }
        public List<DatasetMetadata> Metadatas { get; set; }
    }
}
