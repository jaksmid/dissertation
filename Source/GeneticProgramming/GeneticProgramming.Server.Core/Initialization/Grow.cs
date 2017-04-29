using System;
using System.Collections.Generic;
using GeneticProgramming.Server.Core.GeneticProgramming;
using GeneticProgramming.Server.Core.Helpers;
using GeneticProgramming.Server.Core.Operators;
using GeneticProgramming.Server.Core.Programs;

namespace GeneticProgramming.Server.Core.Initialization
{
    /// <summary>
    /// Grow intialization
    /// </summary>
    public class Grow : IProgramInitializator
    {
        public List<ProgramTypeSet> TypeSets { get; set; }

        public Grow(List<ProgramTypeSet> typeSets)
        {
            TypeSets = typeSets;
        }

        /// <summary>
        /// Random number generator
        /// </summary>
        Random rnd
        {
            get { return RandomHelpers.rnd; }
        }

        public List<GpProgram> CreatePrograms(int maxDepth)
        {
            var fronta = new Queue<Operator>();
            var toReturn = new List<GpProgram>();
            foreach (var programTypeSet in TypeSets)
            {
                List<BaseOperatorTemplate> operators = programTypeSet.OperatorTemplates;
                var gpProgram = new GpProgram();
                toReturn.Add(gpProgram);
                gpProgram.OperatorInstance = operators[rnd.Next(operators.Count)].GetOperatorInstance();
                fronta.Enqueue(gpProgram.OperatorInstance);
                for (int i = 0; i < maxDepth - 1; i++)
                {
                    var frontanew = new Queue<Operator>();
                    foreach (var op in fronta)
                    {
                        for (int k = 0; k < op.Arity; k++)
                        {
                            Operator o = operators[rnd.Next(operators.Count)].GetOperatorInstance();
                            frontanew.Enqueue(o);
                            op.Children.Add(o);
                            o.Parent = op;
                        }
                    }
                    fronta = frontanew;
                }
                foreach (var op in fronta)
                {
                    var terminalCount = programTypeSet.GetTerminals.Count;
                    for (int k = 0; k < op.Arity; k++)
                    {
                        Operator o = programTypeSet.GetTerminals[rnd.Next(terminalCount)].GetOperatorInstance();
                        op.Children.Add(o);
                        o.Parent = op;
                    }
                }
                fronta.Clear();
            }
            return toReturn;
        }
    }
}