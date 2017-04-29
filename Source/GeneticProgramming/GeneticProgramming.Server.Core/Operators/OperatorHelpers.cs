using System.Collections.Generic;

namespace GeneticProgramming.Server.Core.Operators
{
    public class OperatorHelpers
    {
        //TODO: get rid of this, add to config, use operator template factory instead
        public static void AddCommonOperators(List<BaseOperatorTemplate> operators)
        {
            BaseOperatorTemplate op = new IntTemplate("int", new List<string> {"0", "0", "10"});
            operators.Add(op);

            op = new BaseOperatorTemplate("-", new List<string> {"2"});
            operators.Add(op);

            op = new BaseOperatorTemplate("*", new List<string> {"2"});
            operators.Add(op);

            op = new BaseOperatorTemplate("/", new List<string> {"2"});
            operators.Add(op);

            op = new BaseOperatorTemplate("+", new List<string> {"2"});
            operators.Add(op);

            op = new BaseOperatorTemplate("le", new List<string> {"4"});
            operators.Add(op);

            op = new BaseOperatorTemplate ("l", new List<string> {"4"});
            operators.Add(op);

            op = new BaseOperatorTemplate("root", new List<string> {"1"});
            operators.Add(op);

            op = new BaseOperatorTemplate ("log2", new List<string> {"1"});
            operators.Add(op);

            op = new IntTemplate("FR", new List<string> {"0", "0", "2"});
            operators.Add(op);

            op = new IntTemplate("EN", new List<string> {"0", "0", "2"});
            operators.Add(op);

            op = new IntTemplate("SR", new List<string> {"0", "0", "2"});
            operators.Add(op);

            op = new IntTemplate("PR", new List<string> {"0", "0", "2"});
            operators.Add(op);

            op = new IntTemplate("MV", new List<string> {"0", "0", "2"});
            operators.Add(op);

            op = new IntTemplate("CT", new List<string> {"0", "0", "2"});
            operators.Add(op);
        }
    }
}