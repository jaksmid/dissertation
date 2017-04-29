namespace GeneticProgramming.Data.Reporting
{
    /// <summary>
    /// Every generation has fitness and evaluation of the best individual
    /// </summary>
    public class CommonGenerationProgress : IGenerationProgress
    {
        public double Fitness { get; set; }
        public double Validation { get; set; }

        public CommonGenerationProgress(double fitness, double validation)
        {
            Fitness = fitness;
            Validation = validation;
        }

        public string ToJsonString()
        {
            return string.Format("[{0},{1}]", Fitness.ToGBString(), Validation.ToGBString());
        }
    }
}