using System;
using System.Collections.Generic;
using GeneticProgramming.Core.Programs.Terminals;
using GeneticProgramming.Data.Contracts;

namespace GeneticProgramming.Core.Programs
{
    public class ProgramConverter
    {
        public static IProgramNode Convert(Program p, Dictionary<string, Func<string, double>> terminalDictionary)
        {
            IProgramNode toReturn;
            if (p.Label == "int") toReturn = new NodeDoubleConst(int.Parse(p.Value));
            else if (p.Label == "double") toReturn = new NodeDoubleConst(double.Parse(p.Value));
            else if (p.Value == "*") toReturn = new TimesNode(p, terminalDictionary);
            else if (p.Value == "+") toReturn = new PlusNode(p, terminalDictionary);
            else if (p.Value == "-") toReturn = new MinusNode(p, terminalDictionary);
            else if (p.Value == "max") toReturn = new MaxNode(p, terminalDictionary);
            else if (p.Value == "/") toReturn = new DivisionNode(p, terminalDictionary);
            else if (p.Value == "l") toReturn = new LessNode(p, terminalDictionary);
            else if (p.Value == "le") toReturn = new LessOrEqualNode(p, terminalDictionary);
            else if (p.Value == "root") toReturn = new RootNode(p, terminalDictionary);
            else if (p.Value == "log2") toReturn = new LogNode(p, terminalDictionary);
            else if (p.Value == "abs") toReturn = new AbsNode(p, terminalDictionary);
            else toReturn = new TerminalNode<string>(terminalDictionary[p.Label], p.Label, p.Value);
            if (!toReturn.RequiresArgument())
            {
                return new NodeDoubleConst(toReturn.CurrentValue);
            }
            return toReturn;
        }
    }
}
