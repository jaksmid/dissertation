using Metadata.Global;

namespace Metadata.Serialization
{
    public interface IMetadataSerializer
    {
        MetadataCollection Deserialize(string sourceFilaName);
        void Serialize(MetadataCollection source, string targetFileName);
    }
}
