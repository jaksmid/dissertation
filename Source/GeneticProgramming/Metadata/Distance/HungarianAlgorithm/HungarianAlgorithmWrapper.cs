using System;
using System.Collections.Generic;
using System.Linq;
using Metadata.Attributes;

namespace Metadata.Distance.HungarianAlgorithm
{
    public class HungarianAlgorithmWrapper
    {
        private readonly IAttributeMetric _attributeMetric;
        private readonly bool _repairToSemimetric;
        private readonly bool _divideByNumberOfAttributes=false;
        private int _scalingFactor = 100;

        public HungarianAlgorithmWrapper(IAttributeMetric attributeMetric, bool repairToSemimetric=false)
        {
            _attributeMetric = attributeMetric;
            _repairToSemimetric = repairToSemimetric;
        }

        public double ComputeDistance(
            IEnumerable<AttributeMetadata> attributesA, IEnumerable<AttributeMetadata> attributesB)
        {
            var listtA = attributesA.ToList();
            var listtB = attributesB.ToList();
            var listToAddDummy = listtA.Count > listtB.Count ? listtB : listtA;
            var nrToAdd = Math.Abs(listtA.Count - listtB.Count);
            for (int i = 0; i < nrToAdd; i++)
            {
                listToAddDummy.Add(new DummyAttribute());
            }
            int dimension = listtA.Count;
            if (dimension == 0)
            {
                return 0;
            }
            checked
            {
                var distanceMatrix = CreateDistanceMatrix(listtA, listtB);
                //this one will be destroyed
                var distanceMatrixCopy = new int[dimension, dimension];
                Array.Copy(distanceMatrix, distanceMatrixCopy, distanceMatrix.Length);
                var assigments = distanceMatrixCopy.FindAssignments();
                double totalDistance = 0;
                for (int i = 0; i < assigments.Length; i++)
                {
                    totalDistance += distanceMatrix[i, assigments[i]];
                }
                if (_divideByNumberOfAttributes)
                {
                    totalDistance = totalDistance / dimension;
                }
                if (totalDistance < 0)
                {
                    int k = 0;
                }
                return totalDistance / _scalingFactor;
            }
        }

        public IAttributeMetric AttributeMetric
        {
            get { return _attributeMetric; }
        }

        private int[,] CreateDistanceMatrix(List<AttributeMetadata> listA, List<AttributeMetadata> listB)
        {
            int dimension = listA.Count;
            var toReturn = new int[dimension, dimension];
            for (int i=0;i<dimension;i++)
            {
                for (int j=0;j<dimension;j++)
                {
                    //TODO: replace with kernelization
                    if (_repairToSemimetric)
                    {
                        var leftDistance = _attributeMetric.GetDistance(listA[i], listB[j]);
                        var rightDistance = _attributeMetric.GetDistance(listB[j], listA[i]);
                        var absLeftDistance = Math.Abs(leftDistance);
                        var absRightDistance = Math.Abs(rightDistance);
                        var symmetricDistance = (absLeftDistance + absRightDistance)/2;
                        var scaledDistance = (int)(symmetricDistance * _scalingFactor);
                        toReturn[i, j] = scaledDistance;
                    }
                    else
                    {
                        var temp = _attributeMetric.GetDistance(listA[i], listB[j]);
                        var scaledDistance = (int)(temp * _scalingFactor);
                        toReturn[i, j] = scaledDistance;
                    }
                }
            }
            return toReturn;
        }
    }
}
