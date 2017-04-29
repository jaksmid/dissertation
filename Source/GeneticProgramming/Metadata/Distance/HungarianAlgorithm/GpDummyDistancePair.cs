using Metadata.Attributes;

namespace Metadata.Distance.HungarianAlgorithm
{
    public class GpDummyDistancePair
    {
        public IGpDummyDistance<CategoricalMetadata> CategoricalDummyDistance { get; set; }
        public IGpDummyDistance<NumericalAttribute> NumericalDummyDistance { get; set; }

        public GpDummyDistancePair(IGpDummyDistance<CategoricalMetadata> categoricalDummyDistance,
            IGpDummyDistance<NumericalAttribute> numericalDummyDistance)
        {
            CategoricalDummyDistance = categoricalDummyDistance;
            NumericalDummyDistance = numericalDummyDistance;
        }
    }
}