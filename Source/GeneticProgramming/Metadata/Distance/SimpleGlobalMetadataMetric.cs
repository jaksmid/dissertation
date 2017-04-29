using System;
using Metadata.Global;

namespace Metadata.Distance
{
    public class SimpleGlobalMetadataMetric : IMetadataMetric
    {
        public double MeasureDistance(DatasetMetadata datasetA, DatasetMetadata datasetB)
        {
            double distance=0;
            if (datasetA.HasAttributeWithMissingValues != datasetB.HasAttributeWithMissingValues)
            {
                distance++;
            }
            distance += Math.Abs(datasetA.NumberOfCategoricalAttributes - datasetB.NumberOfCategoricalAttributes);
            distance += Math.Abs(datasetA.NumberOfIntegerAttributes - datasetB.NumberOfIntegerAttributes);
            distance += Math.Abs(datasetA.NumberOfRealAttributes - datasetB.NumberOfRealAttributes);

            return distance;
        }
    }
}