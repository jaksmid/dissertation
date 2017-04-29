using Metadata.Global;

namespace Metadata.Import
{
    public interface IMetadataImporter
    {
        DatasetMetadata ImportMetadata(string fileName);
    }
}
