using System;
using System.Collections.Generic;
using GeneticProgramming.Data.Contracts;

namespace GeneticProgramming.Core.Fitness
{
    /// <summary>
    /// Interface for fitness computation
    /// </summary>
    public interface IFitness
    {
        /// <summary>
        /// EValuates computer program
        /// </summary>
        /// <returns>Fitness of program</returns>
        List<Double> Evaluate(Program[] programs);

        List<Double> Validate(Program[] programs);
    }
}
