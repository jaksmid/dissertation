using GeneticProgramming.Data.Dao;
using Metadata.Import;
using Metadata.Prediction.Evaluation;
using Metadata.Results;

namespace GeneticProgramming.ComputationNode.Tasks.Subtasks
{
    public abstract class BaseSubTask:ISubTask
    {
        protected static readonly DbEntitiesProviderFactory DbEntitiesProviderManager=new DbEntitiesProviderFactory();
        protected RankingPredictorEvaluator QualityEvaluator;
        protected IDbEntitiesProvider DbProvider;

        protected BaseSubTask(GlobalMetadataSettings globalMetadataSettings)
        {
            DbProvider = DbEntitiesProviderManager.CreateDbEntitiesProvider();
            var metadataImporter = new DbMetadataImporter(DbProvider, globalMetadataSettings);
            var resultsProvider = new ResultsProvider(DbProvider);
            QualityEvaluator = new RankingPredictorEvaluator(metadataImporter, resultsProvider);
        }

        protected ResultsProvider ResultsProvider { get { return QualityEvaluator.Provider; } }

        public string RunExperiment()
        {
            using (DbProvider)
            {
                return RunExperimentWithOpenConnection();
            }
        }

        public abstract string RunExperimentWithOpenConnection();
    }
}
