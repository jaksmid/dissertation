using System.Collections.Generic;
using GeneticProgramming.Server.Core.GeneticOperators;
using GeneticProgramming.Server.Core.GeneticOperators.Crossover;
using GeneticProgramming.Server.Core.GeneticOperators.Mutation;
using GeneticProgramming.Server.Core.GeneticOperators.Selection;
using GeneticProgramming.Server.Core.GeneticProgramming;
using GeneticProgramming.Server.Core.Initialization;
using GeneticProgramming.Server.Core.Operators;
using GeneticProgramming.Server.Core.Programs;

namespace GeneticProgramming.Server.Core.Bootstrappers
{
    public class AttributeAlignmentBootstrapper : IBootstrapper
    {
        public List<ProgramTypeSet> TypeSets { get; set; }
        /// <summary>
        /// Initialize populations
        /// </summary>
        public void Init(GeneticProgrammingExperiment experiment)
        {
            var populations = new Populations(experiment);
            TypeSets = new List<ProgramTypeSet>
                {
                    new ProgramTypeSet(OperatorsLeft()),
                    new ProgramTypeSet(OperatorsRight())
            };
            populations.Islands = new List<Population>();
            for (int i = 0; i < 1; i++)
            {
                populations.Islands.Add(new Population(500, i, TypeSets, new RampedHalf(TypeSets)) { GeneticOperators = GeneticOperators() });
            }
            experiment.Populations = populations;
        }

        /// <summary>
        /// Gets all operators
        /// </summary>
        /// <returns>OperatorsLeft</returns>
        public List<IGeneticOperator> GeneticOperators()
        {
            var geneticOperators = new List<IGeneticOperator>();
            var bestFinder = new BestIndividualSelector();
            geneticOperators.Add(bestFinder);
            geneticOperators.Add(new TournamentSelection(0.70, 3));
            geneticOperators.Add(new Crossover());
            geneticOperators.Add(new Crossover(1));
            geneticOperators.Add(new Mutation(0.05, new Grow(TypeSets)));
            geneticOperators.Add(new Mutation(0.05, new Grow(TypeSets), 1));
            geneticOperators.Add(new Elitism(bestFinder));
            return geneticOperators;
        }

        public void AddCommonOperators(List<BaseOperatorTemplate> operators)
        {
            BaseOperatorTemplate op = new IntTemplate ("int", new List<string> { "0", "0", "10" } );
            operators.Add(op);

            op = new BaseOperatorTemplate ("-", new List<string> { "2" } );
            operators.Add(op);

            op = new BaseOperatorTemplate ("*", new List<string> { "2" } );
            operators.Add(op);

            op = new BaseOperatorTemplate ("/", new List<string> { "2" } );
            operators.Add(op);

            op = new BaseOperatorTemplate ("+", new List<string> { "2" } );
            operators.Add(op);

            op = new BaseOperatorTemplate ("le",  new List<string> { "4" } );
            operators.Add(op);

            op = new BaseOperatorTemplate ("l", new List<string> { "4" } );
            operators.Add(op);

            op = new BaseOperatorTemplate ("root" , new List<string> { "1" } );
            operators.Add(op);

            op = new BaseOperatorTemplate ("log2" , new List<string> { "1" } );
            operators.Add(op);

            op = new IntTemplate ("FR",  new List<string> { "0", "0", "2" } );
            operators.Add(op);

            op = new IntTemplate ("EN",  new List<string> { "0", "0", "2" } );
            operators.Add(op);

            op = new IntTemplate ("SR",  new List<string> { "0", "0", "2" } );
            operators.Add(op);

            op = new IntTemplate ("PR",  new List<string> { "0", "0", "2" } );
            operators.Add(op);

            op = new IntTemplate ("MV",  new List<string> { "0", "0", "2" } );
            operators.Add(op);

            op = new IntTemplate ("CT",  new List<string> { "0", "0", "2" });
            operators.Add(op);
        }

        public List<BaseOperatorTemplate> OperatorsLeft()
        {
            var operators = new List<BaseOperatorTemplate>();

            AddCommonOperators(operators);

            BaseOperatorTemplate op = new IntTemplate ( "KURT", new List<string> { "0", "0", "2" } );
            operators.Add(op);

            op = new IntTemplate ( "Max", new List<string> { "0", "0", "2" } );
            operators.Add(op);

            op = new IntTemplate ( "Mean", new List<string> { "0", "0", "2" } );
            operators.Add(op);

            op = new IntTemplate ( "Min", new List<string> { "0", "0", "2" } );
            operators.Add(op);

            op = new IntTemplate ( "SK", new List<string> { "0", "0", "2" } );
            operators.Add(op);

            op = new IntTemplate ( "SD", new List<string> { "0", "0", "2" } );
            operators.Add(op);

            op = new IntTemplate ( "VAR", new List<string> { "0", "0", "2" } );
            operators.Add(op);

            op = new IntTemplate ( "IO", new List<string> { "0", "0", "2" } );
            operators.Add(op);

            op = new IntTemplate ( "IU", new List<string> { "0", "0", "2" } );
            operators.Add(op);

            return operators;
        }

        public List<BaseOperatorTemplate> OperatorsRight()
        {
            var operators = new List<BaseOperatorTemplate>();
            
            AddCommonOperators(operators);

            BaseOperatorTemplate op = new IntTemplate ( "NC",  new List<string> { "0", "0", "2" } );
            operators.Add(op);

            op = new IntTemplate ( "UD",  new List<string> { "0", "0", "2" } );
            operators.Add(op);

            op = new IntTemplate ( "TU",  new List<string> { "0", "0", "2" } );
            operators.Add(op);

            op = new IntTemplate ("TK",  new List<string> { "0", "0", "2" } );
            operators.Add(op);

            return operators;
        }
    }
}