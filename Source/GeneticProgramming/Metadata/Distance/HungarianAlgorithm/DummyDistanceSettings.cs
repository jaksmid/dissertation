using Metadata.Global;
using Newtonsoft.Json.Linq;

namespace Metadata.Distance.HungarianAlgorithm
{
    public class DummyDistanceSettings
    {
        public DummyDistanceFactory DummyDistanceFactory { get; set; }

        public DummyDistanceSettings(JToken config, MetadataCollection metadata)
        {
            var isConstant = config.Value<bool>("DummyDistanceConstant");
            if (isConstant)
            {
                double constValue = config.Value<double>("DummyDistanceConstantValue");
                DummyDistanceFactory = new ConstantDummyDistanceFactory(constValue);
            }
            else
            {
                DummyDistanceFactory = new FromAttributeDummyDistanceFactory(metadata);
            }
        }
    }
}