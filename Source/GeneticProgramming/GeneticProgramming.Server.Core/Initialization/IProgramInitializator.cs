using System.Collections.Generic;
using GeneticProgramming.Server.Core.GeneticProgramming;

namespace GeneticProgramming.Server.Core.Initialization
{
    public interface IProgramInitializator
    {
        List<GpProgram> CreatePrograms(int maxDepth);
    }
}
