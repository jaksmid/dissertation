namespace Metadata.Distance.Kernelization
{
    public interface IKernelization
    {
        void FixMatrix(DistanceMatrix matrix);

        double[,] FixValues(double[,] matrix);
    }
}
