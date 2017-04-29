using System;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Factorization;

namespace Metadata.Distance.Kernelization
{
    /// <summary>
    ///  
    /// </summary>
    public class SpectrumFlipKernelization : EigenValuesKernelization
    {
        public SpectrumFlipKernelization(IKernelization predecessor) : base(predecessor)
        {
            
        }

        public override double[,] FixValues(double[,] distances, Evd<Double> evd)
        {
            var modifiedValues = evd.EigenValues.Select(x => Math.Abs(x.Real)).ToArray();
            var diagonal = Matrix.Build.DenseOfDiagonalArray(modifiedValues);
            var result = evd.EigenVectors.Multiply(diagonal.Multiply(evd.EigenVectors.Transpose()));
            var toReturn = result.ToArray();
            return toReturn;
        }
    }
}