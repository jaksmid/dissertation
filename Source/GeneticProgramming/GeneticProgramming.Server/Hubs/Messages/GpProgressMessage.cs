namespace GeneticProgramming.Server.Hubs.Messages
{
    public class GpProgressMessage
    {
        public string ExperimentName { get; set; }
        public int Evaluated { get; set; }
        public int ToEvaluate { get; set; }
        public int Validated { get; set; }
        public int ToValidate { get; set; }
        public int GenerationNumber { get; set; }
    }
}