﻿using System;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Factorization;

namespace Metadata.Distance.Kernelization
{
    /// <summary>
    /// Let Λclip = diag (max(λ1, 0),..., max(λn, 0)) ,and the modified PSD similarity matrix be Sclip = UTΛclipU  
    /// </summary>
    public class SpectrumClipKernelization : EigenValuesKernelization
    {
        public SpectrumClipKernelization(IKernelization predecessor):base(predecessor)
        {
            
        }

        public override double[,] FixValues(double[,] distances, Evd<Double> evd)
        {
            var modifiedValues = evd.EigenValues.Select(x => Math.Max(0, x.Real)).ToArray();
            var diagonal = Matrix.Build.DenseOfDiagonalArray(modifiedValues);
            var result = evd.EigenVectors.Multiply(diagonal.Multiply(evd.EigenVectors.Transpose()));
            var toReturn = result.ToArray();
            return toReturn;
        }
    }
}