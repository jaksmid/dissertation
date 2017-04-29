using System.Collections.Generic;
using GeneticProgramming.Server.Core.GeneticOperators;
using GeneticProgramming.Server.Core.GeneticOperators.Crossover;
using GeneticProgramming.Server.Core.GeneticOperators.Mutation;
using GeneticProgramming.Server.Core.GeneticOperators.Processing;
using GeneticProgramming.Server.Core.GeneticOperators.Selection;
using GeneticProgramming.Server.Core.GeneticProgramming;
using GeneticProgramming.Server.Core.Initialization;
using GeneticProgramming.Server.Core.Operators;
using GeneticProgramming.Server.Core.Programs;

namespace GeneticProgramming.Server.Core.Bootstrappers
{
    public class TestBootstrapper : BaseBootstrapper
    {
        public TestBootstrapper(int populationSize, int populationsCount): base(populationSize, populationsCount)
        {
            
        }

        public List<ProgramTypeSet> TypeSets { get; set; }

        /// <summary>
        /// Initialize populations
        /// </summary>
        public override void Init(GeneticProgrammingExperiment experiment)
        {
            experiment.EvaluationHistory = new History(PopulationsCount, PopulationSize);
            var populations = new Populations(experiment);
            TypeSets = new List<ProgramTypeSet>
            {
                new ProgramTypeSet(CommonOperators())
            };
            populations.Islands = new List<Population>();
            for (int i = 0; i < PopulationsCount; i++)
            {
                populations.Islands.Add(new Population(PopulationSize, i, TypeSets, new RampedHalf(TypeSets)) { GeneticOperators = GeneticOperators(experiment) });
            }
            experiment.Populations = populations;
        }

        /// <summary>
        /// Gets all operators
        /// </summary>
        /// <returns>OperatorsLeft</returns>
        public override List<IGeneticOperator> GeneticOperators(GeneticProgrammingExperiment experiment)
        {
            var geneticOperators = new List<IGeneticOperator>();
            var bestFinder = new BestIndividualSelector();
            geneticOperators.Add(bestFinder);
            geneticOperators.Add(new TournamentSelection(0.70, 3));
            geneticOperators.Add(new Crossover());
            geneticOperators.Add(new Mutation(0.05, new Grow(TypeSets)));
            geneticOperators.Add(new Elitism(bestFinder));
            geneticOperators.Add(new GenerationPostprocess(false, experiment.EvaluationHistory, experiment.RatingQueue));
            return geneticOperators;
        }

        public List<BaseOperatorTemplate> CommonOperators()
        {
            var operators = new List<BaseOperatorTemplate>();
            BaseOperatorTemplate op = new IntTemplate ( "int", new List<string> { "0", "0", "10" } );
            operators.Add(op);

            op = new BaseOperatorTemplate ("-", new List<string> { "2" });
            operators.Add(op);

            op = new BaseOperatorTemplate ("*", new List<string> { "2" });
            operators.Add(op);

            op = new BaseOperatorTemplate ("/", new List<string> { "2" });
            operators.Add(op);

            op = new BaseOperatorTemplate ("+", new List<string> { "2" });
            operators.Add(op);

            op = new BaseOperatorTemplate( "x", new List<string> { "0" });
            operators.Add(op);

            op = new BaseOperatorTemplate ("y", new List<string> { "0" });
            operators.Add(op);
            return operators;
        }
    }
}