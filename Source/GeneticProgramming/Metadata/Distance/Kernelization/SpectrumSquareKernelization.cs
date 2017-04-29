using MathNet.Numerics.LinearAlgebra.Double;

namespace Metadata.Distance.Kernelization
{
    public class SpectrumSquareKernelization : BaseKernelization
    {
        public SpectrumSquareKernelization(IKernelization predecessor):base(predecessor)
        {
            
        }

        public override double[,] GetUpdatedValues(double[,] distances)
        {
            var m =Matrix.Build.DenseOfArray(distances);
            var transposed = m.Transpose();
            var result = m.Multiply(transposed);
            var toReturn = result.ToArray();
            return toReturn;
        }
    }
}