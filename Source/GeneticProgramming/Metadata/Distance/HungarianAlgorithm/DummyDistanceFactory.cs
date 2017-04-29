using System.Collections.Generic;
using Metadata.Attributes;

namespace Metadata.Distance.HungarianAlgorithm
{
    public abstract class DummyDistanceFactory
    {
        public int WeightsNeeded { get; set; }

        public DummyDistancePair CreateDummyDistances(List<double> weights)
        {
            return new DummyDistancePair(GetCategoricalDummyDistance(weights), GetNumericalDummyDistance(weights));
        }

        public GpDummyDistancePair CreateGpDummyDistances()
        {
            return new GpDummyDistancePair(GetCategoricalGpDummyDistance(), GetNumericalGpDummyDistance());
        }

        public abstract IDummyDistance<CategoricalMetadata> GetCategoricalDummyDistance(List<double> weights);

        public abstract IDummyDistance<NumericalAttribute> GetNumericalDummyDistance(List<double> weights);

        public abstract IGpDummyDistance<CategoricalMetadata> GetCategoricalGpDummyDistance();

        public abstract IGpDummyDistance<NumericalAttribute> GetNumericalGpDummyDistance();
    }
}