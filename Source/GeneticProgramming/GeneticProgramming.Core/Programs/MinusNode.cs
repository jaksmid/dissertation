﻿using System;
using System.Collections.Generic;
using GeneticProgramming.Data.Contracts;

namespace GeneticProgramming.Core.Programs
{
    public class MinusNode : BinaryNode
    {
        public MinusNode(Program root, Dictionary<string, Func<string, double>> terminalDictionary)
            : base(root, terminalDictionary)
        {
            
        }

        public override  double CurrentValue
        {
            get { return _left.CurrentValue - _right.CurrentValue; }
        }
    }
}