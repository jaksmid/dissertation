using System;

namespace GeneticProgramming.Data.Models
{
    public class Log
    {
        public int Id { get; set; }
        public string Source { get; set; }

        public string Experiment { get; set; }

        public string Message { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
