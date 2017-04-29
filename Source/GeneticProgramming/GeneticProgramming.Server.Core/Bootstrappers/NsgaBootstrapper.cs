using System;
using System.Collections.Generic;
using GeneticProgramming.Server.Core.GeneticOperators;
using GeneticProgramming.Server.Core.GeneticOperators.Comparers;
using GeneticProgramming.Server.Core.GeneticOperators.Crossover;
using GeneticProgramming.Server.Core.GeneticOperators.Mutation;
using GeneticProgramming.Server.Core.GeneticOperators.Reduction;
using GeneticProgramming.Server.Core.GeneticOperators.Selection;
using GeneticProgramming.Server.Core.GeneticProgramming;
using GeneticProgramming.Server.Core.Initialization;
using GeneticProgramming.Server.Core.Nsga;
using GeneticProgramming.Server.Core.Operators;
using GeneticProgramming.Server.Core.Programs;

namespace GeneticProgramming.Server.Core.Bootstrappers
{
    public class NsgaBootstrapper : IBootstrapper
    {
        public override string ToString()
        {
            return
                String.Format("NsgaBootstrapper - two program type sets - numerical + categorical. Standard operator set");
        }

        /// <summary>
        /// Initialize populations
        /// </summary>
        public void Init(GeneticProgrammingExperiment experiment)
        {
            var pops = new Populations(experiment);
            var programTypeSets = new List<ProgramTypeSet>
                {
                    new ProgramTypeSet(OperatorsLeft()),
                    new ProgramTypeSet(OperatorsRight())
            };
            pops.Islands = new List<Population>();
            for (int i = 0; i < 1; i++)
            {
                pops.Islands.Add(new Population(200, i, programTypeSets, new RampedHalf(programTypeSets)) { GeneticOperators = GeneticOperators() });
            }
            experiment.Populations = pops;
        }

        /// <summary>
        /// Gets all operators
        /// </summary>
        /// <returns>OperatorsLeft</returns>
        public List<IGeneticOperator> GeneticOperators()
        {
            var geneticOperators = new List<IGeneticOperator>();
            //Evaluate all individuals
            //rank the individuals
            geneticOperators.Add(new CrowdingInitOperator());

            var comparator = new NsgaComparator();
            //Reduce the population to its original size based on the elitism
            geneticOperators.Add(new ElitismReduction(comparator));

            //Increase the population size and choose the offspring
            geneticOperators.Add(new NonDestructiveTournamentSelection(0.70, 3, comparator));

            //CrossOver the offspring
            geneticOperators.Add(new NsgaCrossover(0));
            geneticOperators.Add(new NsgaCrossover(1));

            //Mutate the offspring
            geneticOperators.Add(new NsgaMutation(null, 0));
            geneticOperators.Add(new NsgaMutation(null, 1));
            return geneticOperators;
        }

        public List<BaseOperatorTemplate> OperatorsLeft()
        {
            var operators = new List<BaseOperatorTemplate>();

            OperatorHelpers.AddCommonOperators(operators);

            BaseOperatorTemplate op = new IntTemplate("KURT", new List<string> { "0", "0", "2" } );
            operators.Add(op);

            op = new IntTemplate ( "Max",  new List<string> { "0", "0", "2" } );
            operators.Add(op);

            op = new IntTemplate ( "Mean", new List<string> { "0", "0", "2" } );
            operators.Add(op);

            op = new IntTemplate ( "Min",  new List<string> { "0", "0", "2" } );
            operators.Add(op);

            op = new IntTemplate ( "SK", new List<string> { "0", "0", "2" } );
            operators.Add(op);

            op = new IntTemplate ( "SD", new List<string> { "0", "0", "2" } );
            operators.Add(op);

            op = new IntTemplate ( "VAR",  new List<string> { "0", "0", "2" } );
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
            
            OperatorHelpers.AddCommonOperators(operators);

            BaseOperatorTemplate op = new IntTemplate ("NC",  new List<string> { "0", "0", "2" } );
            operators.Add(op);

            op = new IntTemplate ( "UD",  new List<string> { "0", "0", "2" } );
            operators.Add(op);

            op = new IntTemplate ( "TU",  new List<string> { "0", "0", "2" } );
            operators.Add(op);

            op = new IntTemplate ( "TK",  new List<string> { "0", "0", "2" } );
            operators.Add(op);

            return operators;
        }
    }
}