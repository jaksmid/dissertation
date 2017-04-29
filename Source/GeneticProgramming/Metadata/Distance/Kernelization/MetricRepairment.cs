using System;

namespace Metadata.Distance.Kernelization
{
    public class MetricRepairment : BaseKernelization
    {
        public MetricRepairment(IKernelization predecessor):base(predecessor)
        {
            
        }

        public override double[,] GetUpdatedValues(double[,] distances)
        {
            int dim = distances.GetLength(0);
            var result = new double[dim, dim];
            for (int i = 0; i < dim; i++)
            {
                for (int j = 0; j < dim; j++)
                {
                    if (i == j) result[i, i] = 0;
                    else
                    {
                        var a = distances[i, j];
                        var b = distances[j, i];
                        result[i, j] = Math.Abs((a + b)/2);
                    }
                }
            }
            return result;
        }
    }
}