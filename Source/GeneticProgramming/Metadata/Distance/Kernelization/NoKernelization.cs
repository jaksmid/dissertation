namespace Metadata.Distance.Kernelization
{
    public class NoKernelization : IKernelization
    {
        public double[,] FixValues(double[,] matrix)
        {
            return matrix;
        }

        void IKernelization.FixMatrix(DistanceMatrix matrix)
        {
            
        }
    }
}