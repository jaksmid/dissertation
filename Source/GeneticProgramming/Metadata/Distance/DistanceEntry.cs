using Metadata.Global;

namespace Metadata.Distance
{
    public class DistanceEntry
    {
        private readonly DatasetMetadata _originMetadata;
        private readonly DatasetMetadata _targetMetadata;

        public DistanceEntry(DatasetMetadata originMetadata, DatasetMetadata targetMetadata, double distance)
        {
            _originMetadata = originMetadata;
            _targetMetadata = targetMetadata;
            Distance = distance;
        }

        public string SourceName
        {
            get { return _originMetadata.Name; }
        }

        public string TargetName
        {
            get { return _targetMetadata.Name; }
        }

        public DatasetMetadata OriginMetadata
        {
            get { return _originMetadata; }
        }

        public DatasetMetadata TargetMetadata
        {
            get { return _targetMetadata; }
        }

        public double Distance { get; set; }
    }
}
