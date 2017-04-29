using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Factorization;

namespace Metadata.Distance.Kernelization
{
    public abstract class EigenValuesKernelization : BaseKernelization
    {
        protected EigenValuesKernelization(IKernelization predecessor):base(predecessor)
        {
            
        }

        public abstract double[,] FixValues(double[,] distances, Evd<double> decomposition);

        public override double[,] GetUpdatedValues(double[,] distances)
        {
            var m =Matrix.Build.DenseOfArray(distances);
            var evd = m.Evd(Symmetricity.Symmetric);
            var result = FixValues(distances, evd);
            return result;
        }
    }
}