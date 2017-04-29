using System.IO;
using GeneticProgramming.Data.Contracts;
using GeneticProgramming.Data.Models;
using Metadata.Global;
using Newtonsoft.Json;
using Polenter.Serialization;
using DatasetMetadata = Metadata.Global.DatasetMetadata;

namespace Metadata.Serialization
{
    public class SharpMetadataSerializer : IMetadataSerializer
    {
        private readonly SharpSerializer _serializer = new SharpSerializer();

        public MetadataCollection Deserialize(string sourceFilaName)
        {
            // deserialize
            return (MetadataCollection) _serializer.Deserialize(sourceFilaName);
        }

        public void Serialize(MetadataCollection source, string targetFileName)
        {
            // serialize
            _serializer.Serialize(source, targetFileName);
        }

        public string SerializeToJson(DatasetMetadata source)
        {
            var toReturn = JsonConvert.SerializeObject(source, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });
            return toReturn;
        }

        public DatasetMetadata SerializeFromJson(MetadataWithResults source)
        {
            var deserializedProduct = JsonConvert.DeserializeObject<DatasetMetadata>(source.AttributeMetadata, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });
            deserializedProduct.Name = source.DatasetName;
            deserializedProduct.Hash = source.Hash;
            return deserializedProduct;
        }

        public string SerializeToString(DatasetMetadata source)
        {
            using (var ms = new MemoryStream())
            {
                var sw = new StreamWriter(ms);
                _serializer.Serialize(source, ms);
                // The string is currently stored in the 
                // StreamWriters buffer. Flushing the stream will 
                // force the string into the MemoryStream.
                sw.Flush();

                // If we dispose the StreamWriter now, it will close 
                // the BaseStream (which is our MemoryStream) which 
                // will prevent us from reading from our MemoryStream
                //DON'T DO THIS - sw.Dispose();

                // The StreamReader will read from the current 
                // position of the MemoryStream which is currently 
                // set at the end of the string we just wrote to it. 
                // We need to set the position to 0 in order to read 
                // from the beginning.
                ms.Position = 0;
                var sr = new StreamReader(ms);
                var myStr = sr.ReadToEnd(); // serialize

                return myStr;
            }
        }
    }
}