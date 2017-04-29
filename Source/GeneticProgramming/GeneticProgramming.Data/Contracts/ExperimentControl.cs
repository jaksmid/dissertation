using System;

namespace GeneticProgramming.Data.Contracts
{
    public class ExperimentControl
    {
        public String ExperimentName { get; set; }
        public int EndGeneration { get; set; }

        public int ValidationPeriod { get; set; }
    }
}
