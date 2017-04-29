using System.Runtime.Serialization;

namespace GeneticProgramming.Data.Contracts
{
    [DataContract]
    public class BestResult
    {
        [DataMember]
        public string AgentType { get; set; }

        [DataMember]
        public string DatasetName { get; set; }

        [DataMember]
        public double? Accuracy { get; set; }

        [DataMember]
        public long DatasetID { get; set; }
    }
}