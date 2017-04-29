using System.Collections.Generic;
using Metadata.Attributes;

namespace Metadata.Distance.HungarianAlgorithm
{
    public class ConstantDummyDistanceFactory : DummyDistanceFactory
    {
        public double Constant { get; set; }

        public ConstantDummyDistanceFactory(double constant)
        {
            Constant = constant;
            WeightsNeeded = 2;
        }

        public override IDummyDistance<CategoricalMetadata> GetCategoricalDummyDistance(List<double> weights)
        {
            return new ConstantDummyDistance<CategoricalMetadata>(weights[0]);
        }

        public override IDummyDistance<NumericalAttribute> GetNumericalDummyDistance(List<double> weights)
        {
            return new ConstantDummyDistance<NumericalAttribute>(weights[1]);
        }

        public override IGpDummyDistance<CategoricalMetadata> GetCategoricalGpDummyDistance()
        {
            return new ConstantGpDummyDistance<CategoricalMetadata>();
        }

        public override IGpDummyDistance<NumericalAttribute> GetNumericalGpDummyDistance()
        {
            return new ConstantGpDummyDistance<NumericalAttribute>();
        }
    }
}