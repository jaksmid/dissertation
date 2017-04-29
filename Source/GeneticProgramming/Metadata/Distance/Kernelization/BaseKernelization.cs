namespace Metadata.Distance.Kernelization
{
    public abstract class BaseKernelization: IKernelization
    {
        public IKernelization PredecessingKernelization { get; set; }

        protected BaseKernelization()
        {
            
        }

        protected BaseKernelization(IKernelization predecessor)
        {
            PredecessingKernelization = predecessor;
        }

        public double[,] ExtractDistances(DistanceEntry[,] entries)
        {
            int dim = entries.GetLength(0);
            var result = new double[dim, dim];
            for (int i = 0; i < dim; i++)
            {
                for (int j = 0; j < dim; j++)
                {
                    result[i, j] = entries[i, j].Distance;
                }
            }
            return result;
        }

        public void UpdateDistances(double[,] newDistances, DistanceEntry[,] entries)
        {
            int dim = entries.GetLength(0);
            for (int i = 0; i < dim; i++)
            {
                for (int j = 0; j < dim; j++)
                {
                    entries[i, j].Distance = newDistances[i, j];
                }
            }
        }

        public abstract double[,] GetUpdatedValues(double[,] distances);


        public void FixMatrix(DistanceMatrix matrix)
        {
            DistanceEntry[,] entries = matrix.DistanceEntries;
            var distances = ExtractDistances(entries);
            var result = FixValues(distances);
            UpdateDistances(result, entries);
        }

        public double[,] FixValues(double[,] matrix)
        {
            double[,] currentMatrix = matrix;
            if (PredecessingKernelization != null)
            {
                currentMatrix = PredecessingKernelization.FixValues(currentMatrix);
            }
            var result = GetUpdatedValues(currentMatrix);
            return result;
        }
    }
}
