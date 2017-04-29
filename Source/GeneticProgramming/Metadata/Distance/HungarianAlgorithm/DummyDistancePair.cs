using Metadata.Attributes;

namespace Metadata.Distance.HungarianAlgorithm
{
    public class DummyDistancePair
    {
        public IDummyDistance<CategoricalMetadata> CategoricalDummyDistance { get; set; }
        public IDummyDistance<NumericalAttribute> NumericalDummyDistance { get; set; }

        public DummyDistancePair(IDummyDistance<CategoricalMetadata> categoricalDummyDistance,
            IDummyDistance<NumericalAttribute> numericalDummyDistance)
        {
            CategoricalDummyDistance = categoricalDummyDistance;
            NumericalDummyDistance = numericalDummyDistance;
        }
    }
}