using System.Collections.Generic;

namespace Metadata.Distance.Kernelization
{
    public class KernelizationFactory
    {
        public static IKernelization CreateKernelization(KernelizationTypes type, IKernelization predecessor = null)
        {
            switch (type)
            {
                case KernelizationTypes.NoKernelization:
                    return new NoKernelization();
                case KernelizationTypes.SpectrumClipKernelization:
                    return new SpectrumClipKernelization(predecessor);
                case KernelizationTypes.SpectrumFlipKernelization:
                    return new SpectrumFlipKernelization(predecessor);
                case KernelizationTypes.SpectrumShiftKernelization:
                    return new SpectrumShiftKernelization(predecessor);
                case KernelizationTypes.SpectrumSquareKernelization:
                    return new SpectrumSquareKernelization(predecessor);
                case KernelizationTypes.MetricRepairment:
                    return new MetricRepairment(predecessor);
            }
            return null;
        }

        public static IKernelization CreateKernelization(List<KernelizationTypes> types)
        {
            IKernelization current = null;
            foreach (var kernelizationType in types)
            {
                current = CreateKernelization(kernelizationType, current);
            }
            return current;
        }
    }
}