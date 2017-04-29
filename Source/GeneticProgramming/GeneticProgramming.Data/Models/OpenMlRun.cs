using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticProgramming.Data.Models
{
    public class OpenMlRun
    {
        public int OpenMlRunId { get; set; }
        public int DatasetId { get; set; }
        public string Algorithm { get; set; }
        public string Parametres { get; set; }
        public int TaskId { get; set; }
        public double PredictiveAccuracy { get; set; }
        public Double? UserCpuTimeMillis { get; set; }
        public Double? UserCpuTimeMillisTesting { get; set; }
        public Double? UserCpuTimeMillisTraining { get; set; }
        public double? RunCpuTime { get; set; }
        public double? BuildCpuTime { get; set; }
        public double MeanAbsoluteError { get; set; }
    }
}
