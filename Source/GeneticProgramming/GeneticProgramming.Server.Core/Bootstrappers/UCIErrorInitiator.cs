////using System;
////using System.Collections.Generic;
////using System.Linq;
////using System.Web;
////using GeneticProgramming.Server.GeneticOperators;
////using GeneticProgramming.Server.GeneticProgramming;
////using GeneticProgramming.Server.Operators;
////using GeneticProgramming.Server.Genetic_Operators;

////namespace GeneticProgramming.Server.Initiators
////{
////    /// <summary>
////    /// Intializator for UCI fitness
////    /// </summary>
////    public class UCIErrorInitiator//:IBootstrapper
////    {
////        /// <summary>
////        /// Initialize populations
////        /// </summary>
////        /// <param name="pops">Populations to initialize</param>
////        public void Init(GeneticProgramming.Populations pops)
////        {
////            pops.Islands = new List<Population>();

////            //TODO: fix if required pops.InitOperators(OperatorsLeft());

////            for (int i = 0; i < 10; i++)
////            {
////                pops.Islands.Add(new Population(pops, 400, i) { GeneticOperators = GeneticOperators() });
////            }
////            pops.InitedBy = "UCIErrorInitiator";
////        }

////        /// <summary>
////        /// Gets all operators
////        /// </summary>
////        /// <returns>OperatorsLeft</returns>
////        public List<IGeneticOperator> GeneticOperators()
////        {
////            List<IGeneticOperator> geneticOperators = new List<IGeneticOperator>();
////            geneticOperators.Add(new AntiBloatFitnessScaling());
////            geneticOperators.Add(new Elitism());
////            geneticOperators.Add(new TournamentSelection(0.70, 3));
////            geneticOperators.Add(new Crossover());
////            geneticOperators.Add(new Mutation() { MutationChance = 0.05 });
////            return geneticOperators;
////        }

////        /// <summary>
////        /// Gets all genetic operators
////        /// </summary>
////        /// <returns>Genetic operators</returns>
////        public List<BaseOperatorTemplate> Operators()
////        {
////            List<BaseOperatorTemplate> operators = new List<BaseOperatorTemplate>();
////            //BaseOperatorTemplate op = new BaseOperatorTemplate() { Label = "x", Arguments = new List<string>() { "0" } };
////            //operators.Add(op);

////            //op = new BaseOperatorTemplate() { Label = "y", Arguments = new List<string>() { "0" } };
////            //operators.Add(op);

////            BaseOperatorTemplate op = new IntTemplate() { Label = "int", Arguments = new List<string>() { "0", "0", "10" } };
////            operators.Add(op);

////            op = new BaseOperatorTemplate() { Label = "Complexity", Arguments = new List<string>() { "0" } };
////            operators.Add(op);

////            op = new IntTemplate() { Label = "ErrorOnNearTask", Arguments = new List<string>() { "0", "1", "6" } };
////            operators.Add(op);

////            op = new IntTemplate() { Label = "DistanceOnNearTask", Arguments = new List<string>() { "0", "1", "6" } };
////            operators.Add(op);

////            op = new BaseOperatorTemplate() { Label = "+", Arguments = new List<string>() { "2" } };
////            operators.Add(op);

////            op = new BaseOperatorTemplate() { Label = "-", Arguments = new List<string>() { "2" } };
////            operators.Add(op);

////            op = new BaseOperatorTemplate() { Label = "*", Arguments = new List<string>() { "2" } };
////            operators.Add(op);

////            op = new BaseOperatorTemplate() { Label = "/", Arguments = new List<string>() { "2" } };
////            operators.Add(op);

////            op = new BaseOperatorTemplate() { Label = "log2", Arguments = new List<string>() { "1" } };
////            operators.Add(op);

////            op = new BaseOperatorTemplate() { Label = "root", Arguments = new List<string>() { "1" } };
////            operators.Add(op);

////            op = new BaseOperatorTemplate() { Label = "l", Arguments = new List<string>() { "4" } };
////            operators.Add(op);

////            op = new BaseOperatorTemplate() { Label = "le", Arguments = new List<string>() { "4" } };
////            operators.Add(op);

////            return operators;
////        }
////    }
////}