using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeneticProgramming.Data.Contracts;
using GeneticProgramming.Data.Reporting;
using GeneticProgramming.Server.Core.GeneticProgramming;
using GeneticProgramming.Server.Core.Settings;
using log4net;

namespace GeneticProgramming.Server.Core
{
    public class GeneticProgrammingExperiment
    {
        public IGpExperimentSettings Settings { get; set; }
        private static readonly ILog Logger = LogManager.GetLogger(typeof(GeneticProgrammingExperiment));
        public bool IsEvaluationFinished { get; set; }
        private bool _finished;

        public RateIndividualQueue RatingQueue= new RateIndividualQueue();

        public History EvaluationHistory { get; set; }

        /// <summary>
        /// Populations
        /// </summary>
        public Populations Populations { get; set; }

        public GeneticProgrammingExperiment(IGpExperimentSettings settings)
        {
            Settings = settings;
            settings.Bootstrapper.Init(this);
            var message = String.Format("Initiating Genetic Programming Initiator: {0}.", Settings.Bootstrapper);
            Logger.Info(message);
            foreach (var population in Populations.Islands)
            {
                var toAdd = population.Individuals.Select(individual => new RateIndividualTask(individual));
                foreach (var newItem in toAdd)
                {
                    RatingQueue.Enqueue(newItem);
                }
            }
        }

        /// <summary>
        /// Gets non rated individual
        /// </summary>
        /// <returns>Non rated individual in envelop representation</returns>
        public RateIndividualTask GetIndividual()
        {
            while (RatingQueue.Any())
            {
                RateIndividualTask task = RatingQueue.Dequeue();
                RatingQueue.Enqueue(task);
                return task;
            }
            if (!_finished)
            {
                ExperimentFinished();
            }
            return null;
        }

        /// <summary>
        /// Gets non rated individual
        /// </summary>
        /// <returns>Non rated individual in envelop representation</returns>
        public Individual GetSpecificIndividual(int generation, int populationNumber, int individualNumber)
        {
            if (generation == EvaluationHistory.CurrentGenerationNumber)
            {
                return Populations.Islands[populationNumber].Individuals[individualNumber];
            }
            return
                EvaluationHistory.Generations[generation].PopulationHistories[populationNumber].IndividulResults[
                    individualNumber].EvaluatedIndividual;
        }

        /// <summary>
        /// Rate individual
        /// </summary>
        /// <param name="results">Individual avaluation results</param>
        public void RateIndividual(IndividualEvaluationResults results)
        {
            Individual ind;
            if (results.TaskId == -1)
            {
                ind = GetSpecificIndividual(results.GenerationNumber, results.PopulationNumber, results.IndividualNumber);
            }
            else
            {
                var task = RatingQueue.GetById(results.TaskId);
                ind = task.IndividualToRate;
            }
            EvaluationHistory.RateIndividual(ind, results);
            if (EvaluationHistory.CurrentGenerationCompletelyEvaluated && results.MultiObjectiveFitness != null && !IsEvaluationFinished)
            {
                NextGeneration();
            }
        }

        public void ExperimentFinished()
        {
            var generationReporter = new GenerationProgressReporter(Settings.ExperimentName);
            foreach (var generation in EvaluationHistory.Generations)
            {
                IGenerationProgress toAdd;
                if (Settings.MultiObjective)
                {
                    var firstFront =
                        generation.PopulationHistories[0].IndividulResults.Where(x => x.EvaluatedIndividual.Rank == 0).ToList();
                    var firstFrontFitness = firstFront.Select(x => x.Evaluation).ToList();
                    var firstFrontValidation =
                        firstFront.Where(x => x.Validation != null).Select(x => x.Validation).ToList();
                    var maxFitness = generation.MaxFitness.ToArray();
                    var maxFitnessValidation = generation.MaxValidation?.ToArray();
                    toAdd = new MultiobjectiveDetailedGenerationProgress
                    {
                        AverageNumberOfNodes = generation.AverageNodes.ToArray(),
                        FirstFrontFitness = firstFrontFitness,
                        FirstFrontValidation = firstFrontValidation,
                        MaxObjectives = maxFitness,
                        MaxObjectivesValidation = maxFitnessValidation
                    };
                }
                else
                {
                    double? maxValidation = null;
                    if (generation.MaxValidation != null) maxValidation = generation.MaxValidation[0];
                    toAdd = new DetailedGenerationProgress
                    {
                        MaxFitness = generation.MaxFitness[0],
                        AverageFitness = generation.AverageFitness[0],
                        MinFitness = generation.MinFitness[0],
                        AverageNumberOfNodes = generation.AverageNodes.ToArray(),
                        MaxEvaluation = maxValidation
                    };
                }
                generationReporter.AddGeneration(toAdd);
            }
            var result = generationReporter.EndExperiment();
            SaveResultToDb.SaveResult(Settings.ExperimentName, result, "run", Settings.SettingsToString());
            _finished = true;
        }

        public void NextGeneration()
        {
            IsEvaluationFinished = Settings.EndCriterion.IsEvaluationFinished(Populations, EvaluationHistory.CurrentGenerationNumber);
            if (IsEvaluationFinished)
            {
                RatingQueue.Cleanup();
                //last chance to add some individuals to the queue
                var toValidate = Settings.ValidationSelector.GetIndividualToValidate(EvaluationHistory);
                toValidate.ForEach(x => RatingQueue.Enqueue(x));
            }
            else
            {
                //Todo: some other progress can increase pop size
                EvaluationHistory.AddGeneration(Settings.MultiObjective);
                foreach (var globalOperator in Populations.Operators)
                {
                    globalOperator.ModifyPopolation(Populations);
                }
                var toLog = new StringBuilder();
                toLog.Append(string.Format("New generation: {0}.", EvaluationHistory.CurrentGenerationNumber));
                foreach (var max in EvaluationHistory.Generations[EvaluationHistory.CurrentGenerationNumber-1].MaxFitness)
                {
                    toLog.Append(string.Format("Best fitness: {0}", max));
                }
                foreach (var population in Populations.Islands)
                {
                    population.LogStatus(toLog, false);
                }
                Logger.Info(toLog.ToString());
                Logger.Info("******************************************");
            }
        }

        /// <summary>
        /// Number of populations to create
        /// </summary>
        public int NumberOfPopulations
        {
            get { return Populations.Islands.Count; }
        }
    }
}
