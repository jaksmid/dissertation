using System;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Factorization;

namespace Metadata.Distance.Kernelization
{
    public class SpectrumShiftKernelization : EigenValuesKernelization
    {
        public SpectrumShiftKernelization(IKernelization predecessor) : base(predecessor)
        {
            
        }

        public override double[,] FixValues(double[,] distances, Evd<Double> evd)
        {
            var min = Math.Abs(Math.Min(evd.EigenValues.Min(x=>x.Real), 0));
            var modifiedValues = evd.EigenValues.Select(x => x.Real + min).ToArray();
            var diagonal = Matrix.Build.DenseOfDiagonalArray(modifiedValues);
            var result = evd.EigenVectors.Multiply(diagonal.Multiply(evd.EigenVectors.Transpose()));
            var toReturn = result.ToArray();
            return toReturn;
        }
    }
}