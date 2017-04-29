using System.Collections.Generic;
using GeneticProgramming.Server.Core.GeneticOperators;
using GeneticProgramming.Server.Core.GeneticOperators.Comparers;
using GeneticProgramming.Server.Core.GeneticOperators.Crossover;
using GeneticProgramming.Server.Core.GeneticOperators.Mutation;
using GeneticProgramming.Server.Core.GeneticOperators.Processing;
using GeneticProgramming.Server.Core.GeneticOperators.Reduction;
using GeneticProgramming.Server.Core.GeneticOperators.Selection;
using GeneticProgramming.Server.Core.GeneticOperators.Validation;
using GeneticProgramming.Server.Core.GeneticProgramming;
using GeneticProgramming.Server.Core.Initialization;
using GeneticProgramming.Server.Core.Nsga;
using GeneticProgramming.Server.Core.Programs;
using GeneticProgramming.Server.Core.Settings;

namespace GeneticProgramming.Server.Core.Bootstrappers
{
    public class FromSettingBootstrapper : IBootstrapper
    {
        public IGpExperimentSettings Settings { get; set; }

        public FromSettingBootstrapper(IGpExperimentSettings settings)
        {
            Settings = settings;
        }

        /// <summary>
        /// Initialize populations
        /// </summary>
        public void Init(GeneticProgrammingExperiment experiment)
        {
            //todo:add full support for diverse pop sizes
            experiment.EvaluationHistory = new History(Settings.PopulationCount, Settings.PopulationsSettings[0].PopulationSize);
            var populations = new Populations(experiment)
            {
                Islands = new List<Population>()
            };
            var i = 0;
            foreach (var populationsSetting in Settings.PopulationsSettings)
            {
                var typeSets = populationsSetting.TypeSets;
                populations.Islands.Add(new Population(populationsSetting.PopulationSize, i, typeSets, new RampedHalf(typeSets))
                {
                    GeneticOperators = GeneticOperators(experiment, typeSets)
                });
                i++;
            }
            experiment.Populations = populations;
            Settings.InitializationBootstrapper?.Bootstrap(experiment.Populations, Settings);
        }

        /// <summary>
        /// Gets all operators
        /// </summary>
        /// <returns>OperatorsLeft</returns>
        public List<IGeneticOperator> GeneticOperators(GeneticProgrammingExperiment experiment, List<ProgramTypeSet> typeSets)
        {
            var factory = new GeneticOperatorFactory(experiment, typeSets);
            return factory.CreateGeneticOperators(Settings);
        }
    }

    public class GeneticOperatorFactory
    {
        public GeneticProgrammingExperiment Experiment { get; set; }
        public List<ProgramTypeSet> TypeSets { get; set; }
        private Dictionary<string, IGeneticOperator> _namedOperators = new Dictionary<string, IGeneticOperator>(); 

        public GeneticOperatorFactory(GeneticProgrammingExperiment experiment, List<ProgramTypeSet> typeSets)
        {
            Experiment = experiment;
            TypeSets = typeSets;
        }

        public List<IGeneticOperator> CreateGeneticOperators(IGpExperimentSettings settings)
        {
            var geneticOperators = new List<IGeneticOperator>();
            foreach (var geneticOperatorTemplate in settings.Operators)
            {
                IGeneticOperator toAdd = CreateGeneticOperator(geneticOperatorTemplate);
                if (!string.IsNullOrEmpty(geneticOperatorTemplate.Name))
                {
                    _namedOperators.Add(geneticOperatorTemplate.Name,toAdd);
                }
                geneticOperators.Add(toAdd);
            }
            geneticOperators.Add(new GenerationPostprocess(settings.FitnessCanChange, Experiment.EvaluationHistory, Experiment.RatingQueue));
            return geneticOperators;
        } 


        public IGeneticOperator CreateGeneticOperator(GeneticOperatorTemplate operatorTemplate)
        {
            switch (operatorTemplate.Type)
            {
                case "TournamentSelection":
                    var toReturn = new TournamentSelection(
                        operatorTemplate.GetDoubleValue("ProbabilityOfBetterWinning"),
                        operatorTemplate.GetIntValue("TournamentSize"));
                    return toReturn;
                case "AntiBloatTournamentSelection":
                    var antiBloatTournament = new TournamentSelection(
                        operatorTemplate.GetDoubleValue("ProbabilityOfBetterWinning"),
                        operatorTemplate.GetIntValue("TournamentSize"), new AntiBloatFitnessComparer(
                            operatorTemplate.GetIntValue("DepthToPenalize"),
                            operatorTemplate.GetIntValue("WidthToPenalize"),
                            operatorTemplate.GetIntValue("NodesToPenalize"),
                            operatorTemplate.GetIntValue("DepthPenalization"),
                            operatorTemplate.GetIntValue("WidthPenalization"),
                            operatorTemplate.GetIntValue("NodesPenalization")
                            )
                        );
                    return antiBloatTournament;
                case "Crossover":
                    var crossover = new Crossover(operatorTemplate.GetDoubleValue("CrossoverChance"), 
                        operatorTemplate.GetIntValueOrDefault("ProgramSetNumber", 0));
                    return crossover;
                case "Mutation":
                    var mutation = new Mutation(operatorTemplate.GetDoubleValue("MutationChance"),
                        new Grow(TypeSets),
                        operatorTemplate.GetIntValueOrDefault("ProgramSetNumber",0));
                    return mutation;
                case "BestIndividualSelector":
                    return new BestIndividualSelector();
                case "Elitism":
                    return new Elitism((BestIndividualSelector)_namedOperators[operatorTemplate.Dependencies[0]]);
                case "ValidateBest":
                    return new ValidateBest(Experiment.RatingQueue);
                case "CrowdingInitOperator":
                    return new CrowdingInitOperator();
                case "NsgaElitismReduction":
                    var comparator = new NsgaComparator();
                    //Reduce the population to its original size based on the elitism
                    return new ElitismReduction(comparator);
                case "NonDestructiveTournamentSelection":
                    var nsgaComparator = new NsgaComparator();
                    return new NonDestructiveTournamentSelection(operatorTemplate.GetDoubleValue("ProbabilityOfBetterWinning"),
                        operatorTemplate.GetIntValue("TournamentSize"), nsgaComparator);
                case "NsgaCrossover":
                    return new NsgaCrossover(operatorTemplate.GetIntValueOrDefault("ProgramSetNumber", 0));
                case "NsgaMutation":
                    return new NsgaMutation(new Grow(TypeSets), operatorTemplate.GetIntValueOrDefault("ProgramSetNumber", 0));

            }
            return null;
        }
    }
}