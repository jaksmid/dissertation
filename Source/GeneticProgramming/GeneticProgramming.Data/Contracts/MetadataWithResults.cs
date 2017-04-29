using System.Runtime.Serialization;
using GeneticProgramming.Data.Models;

namespace GeneticProgramming.Data.Contracts

{
    [DataContract]
    public class MetadataWithResults
    {

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Hash { get; set; }

        [DataMember]
        public int OpenMlId { get; set; }

        [DataMember]
        public string AttributeMetadata { get; set; }

        [DataMember]
        public string DatasetName { get; set; }
    }
}