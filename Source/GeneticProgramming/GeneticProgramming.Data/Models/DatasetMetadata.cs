namespace GeneticProgramming.Data.Models
{
    public class DatasetMetadata
    {
        public string DatasetName { get; set; }
        public string Hash { get; set; }
        public int OpenMlId { get; set; }
        public string AttributeMetadata { get; set; }

        public int NumberOfAttributes { get; set; }

        public bool IsDisabled { get; set; }

        public bool IsCorrupted { get; set; }

        public bool DeletedInOpenMl { get; set; }
    }
}
