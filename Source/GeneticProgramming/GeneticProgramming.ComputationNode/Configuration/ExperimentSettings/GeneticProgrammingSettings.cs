using System.Collections.Generic;
using System.Linq;
using GeneticProgramming.ComputationNode.Configuration.ExperimentSettings.GenProgramming;
using GeneticProgramming.ComputationNode.Configuration.ExperimentSettings.SubSettings;
using GeneticProgramming.ComputationNode.Tasks.Subtasks;
using GeneticProgramming.Core.Fitness;
using GeneticProgramming.Data.Dao;
using GeneticProgramming.Server.Core;
using GeneticProgramming.Server.Core.Bootstrappers;
using GeneticProgramming.Server.Core.EndCriterions;
using GeneticProgramming.Server.Core.Initialization;
using GeneticProgramming.Server.Core.Programs;
using GeneticProgramming.Server.Core.Settings;
using Newtonsoft.Json.Linq;

namespace GeneticProgramming.ComputationNode.Configuration.ExperimentSettings
{
    public class GeneticProgrammingSettings: BaseSettings, IGpExperimentSettings
    {
        private static readonly DbEntitiesProviderFactory DbEntitiesProviderFactory=new DbEntitiesProviderFactory();
        public ICoevolutionSettings CoevolutionSettings { get; set; }
        public IInitializationBootstrapper InitializationBootstrapper { get; set; }
        public bool MultiObjective { get; set; }

        public GeneticProgrammingSettings(JToken config):base(config)
        {
            FitnessTypes fitnessType = ConfigParser.ParseFitnessType(Config);
            KnnSettings = new KnnSettings(Config);
            CoevolutionSettings = new CoevolutionSettings(Config);
            MultiObjective = false;
            if (config["MultiObjective"] != null)
            {
                MultiObjective = config.Value<bool>("MultiObjective");
            }
            FitnessFunction = FitnessFactory.CreateFitness(fitnessType, DbEntitiesProviderFactory, Config, KnnSettings.K, MultiObjective);
            PopulationSize = ConfigParser.ParsePopulationSize(Config);
            FitnessCanChange = config.Value<bool>("FitnessCanChange");
            var generations = config.Value<int>("Generations");
            EndCriterion = new GenerationEndCriterion(generations);
            if (MultiObjective)
            {
                ValidationSelector = new NonDominatedToValidate();
            }
            else
            {
                ValidationSelector = new BestIndividualsToValidate();
            }
            var populationTemplates = ConfigParser.ParsePopulationsCount(Config);
            PopulationsSettings = new List<PopulationBootstrapSettings>();
            InitializationBootstrapper = InitializationBootstrapperFactory.CreateBootstrapper(config);
            foreach (var populationTemplate in populationTemplates)
            {
                var typeSets = new List<ProgramTypeSet>();
                foreach (var typeSet in populationTemplate.TypeSets)
                {
                    var unwrappedTypeSet = typeSet.Select(BaseoperatorTemplateFactory.CreateTemplate).ToList();
                    typeSets.Add(new ProgramTypeSet(unwrappedTypeSet));
                }
                for (int subPopNr = 0; subPopNr < populationTemplate.PopulationsCount; subPopNr++)
                {
                    var subPop = new PopulationBootstrapSettings(PopulationSize, typeSets);
                    PopulationsSettings.Add(subPop);
                }
            }
            Bootstrapper = new FromSettingBootstrapper(this);
            Operators = ConfigParser.ParseGeneticOperatorTemplates(Config);
        }
        public int PopulationSize { get; set; }

        public bool FitnessCanChange { get; set; }
        public IKnnSettings KnnSettings { get; set; }

        public int PopulationCount
        {
            get { return PopulationsSettings.Count; }
        }

        public List<GeneticOperatorTemplate> Operators { get; set; }
        public string SettingsToString()
        {
            return Config.ToString();
        }

        public override ISubTask CreateSubtask()
        {
            return new GeneticProgrammingSubTask(this);
        }

        public IFitness FitnessFunction { get; set; }

        public override string ExperimentPrefix { get { return "GP"; } }
        public IBootstrapper Bootstrapper { get; set; }
        public IEndCriterion EndCriterion { get; set; }
        public IValidationSelector ValidationSelector { get; set; }

        public string ExperimentName
        {
            get { return CreateExperimentName(); }
        }

        public List<PopulationBootstrapSettings> PopulationsSettings { get; set; }
    }
}
