using System;
using System.Collections.Generic;
using System.Linq;
using GeneticProgramming.Data.Contracts;
using GeneticProgramming.Data.Statistics;
using GeneticProgramming.Server.Core.GeneticProgramming;

namespace GeneticProgramming.Server.Core
{
    public class History
    {
        public int Populations { get; set; }
        public int PopulationSize { get; set; }
        private List<GenerationHistory> _generations = new List<GenerationHistory>();
        public int CurrentGenerationNumber { get; set; }

        public bool CurrentGenerationCompletelyEvaluated { get; set; }

        public List<GenerationHistory> Generations
        {
            get { return _generations; }
            set { _generations = value; }
        }

        public History(int popsCout, int popSize)
        {
            Populations = popsCout;
            PopulationSize = popSize;
            Generations.Add(new GenerationHistory(CurrentGenerationNumber, Populations, PopulationSize));
        }

        public void AddGeneration(bool populationDoubled)
        {
            CurrentGenerationNumber ++;
            CurrentGenerationCompletelyEvaluated = false;
            var sizeOfNewPopulation = PopulationSize;
            if (populationDoubled)
            {
                sizeOfNewPopulation = sizeOfNewPopulation*2;
            }
            Generations.Add(new GenerationHistory(CurrentGenerationNumber, Populations, sizeOfNewPopulation));
        }

        public GenerationHistory CurrentGeneration { get { return Generations[CurrentGenerationNumber]; } }

        public void RateIndividual(Individual ind, IndividualEvaluationResults results = null)
        {
            if (results != null)
            {
                ind.SetResults(results);
            }
            if (ind.Statistics == null || ind.Statistics[0].NumberOfNodes == null)
            {
                var x = 1;
            }
            int generationNumber = ind.GenerationNumber;
            var result = Generations[generationNumber].RateIndividual(ind);
            if (result) CurrentGenerationCompletelyEvaluated = true;
        }
    }

    public class GenerationHistory
    {
        public int GenerationNumber { get; set; }

        public List<double> MaxFitness
        {
            get { return ComputeEvaluationStatistic(x => x.Max(), y => y.MaxFitness); }
        }

        public List<double> MaxValidation
        {
            get { return ComputeEvaluationStatistic(x => x.Max(), y => y.MaxValidation); }
        }

        public List<double> MinFitness
        {
            get { return ComputeEvaluationStatistic(x => x.Min(), y => y.MinFitness); }
        }

        public List<double> AverageFitness
        {
            //TODO: amend for multiple populations
            get { return ComputeEvaluationStatistic(x => x.Average(), y => y.AverageFitness); }
        }

        public List<double> AverageNodes
        {
            //TODO: amend for multiple populations
            get { return ComputeEvaluationStatistic(x => x.Average(), y => y.AverageNodes); }
        }

        private List<double> ComputeEvaluationStatistic(Func<IEnumerable<double>, double> aggregation, Func<PopulationHistory, List<double>> selector )
        {
            var evaluated = PopulationHistories.Where(x => selector(x) != null).ToList();
            if (evaluated.Any())
            {
                int objectives = selector(evaluated.First()).Count;
                var toReturn = new List<double>();
                for (int i = 0; i < objectives; i++)
                {
                    int i1 = i;
                    toReturn.Add(aggregation(evaluated.Select(x => selector(x)[i1])));
                }
                return toReturn;
            }
            return null;
        }

        public List<PopulationHistory> PopulationHistories { get; set; }

        public int Remaining { get; set; }

        public GenerationHistory(int generationNumber, int populations, int populationSize)
        {
            GenerationNumber = generationNumber;
            PopulationHistories = new List<PopulationHistory>(populations);
            for (int population = 0; population < populations; population++)
            {
                PopulationHistories.Add(new PopulationHistory(populationSize));
            }
            Remaining = populations;
        }
        
        public bool RateIndividual(Individual ind)
        {
            int populationNumber = ind.PopulationNumber;
            var popChangedToZero = PopulationHistories[populationNumber].RateIndividual(ind);
            if (popChangedToZero)
            {
                Remaining--;
                return Remaining == 0;
            }
            return false;
        }
    }

    public class PopulationHistory
    {
        private List<double> ComputeEvaluationStatistic(Func<IEnumerable<double>, double> aggregation)
        {
            if (EvaluatedResults.Any())
            {
                int objectives = EvaluatedResults.First().Evaluation.Count;
                var toReturn = new List<double>();
                for (int i = 0; i < objectives; i++)
                {
                    int i1 = i;
                    toReturn.Add(aggregation(EvaluatedResults.Select(x=>x.Evaluation[i1])));
                }
                return toReturn;
            }
            return null;
        }

        public List<double> MaxFitness
        {
            get { return ComputeEvaluationStatistic(x=>x.Max()); }
        }

        public List<double> AverageFitness
        {
            get { return ComputeEvaluationStatistic(x => x.Average()); }
        }

        public List<double> AverageNodes
        {
            get
            {
                if (EvaluatedResults.Any())
                {
                    int programs = EvaluatedResults.First().Statistics.Count();
                    var toReturn = new List<double>();
                    for (int i = 0; i < programs; i++)
                    {
                        int i1 = i;
                        var toAdd = EvaluatedResults.Select(x => x.Statistics[i1].NumberOfNodes.Value).Average();
                        toReturn.Add(toAdd);
                    }
                    return toReturn;
                }
                return null;
            }
        }

        public List<double> MinFitness
        {
            get { return ComputeEvaluationStatistic(x => x.Min()); }
        }

        public List<double> MaxValidation
        {
            get
            {
                if (ValidatedResults.Any())
                {
                    int objectives = ValidatedResults.First().EvaluatedIndividual.MultiObjectiveValidation.Count;
                    var toReturn = new List<double>();
                    for (int i = 0; i < objectives; i++)
                    {
                        toReturn.Add(ValidatedResults.Max(x => x.EvaluatedIndividual.MultiObjectiveValidation[i]));
                    }
                    return toReturn;
                }
                return null;
            }
        }

        public int PopulationSize { get; set; }
        public List<EvaluationEntry> IndividulResults { get; set; }

        public List<EvaluationEntry> EvaluatedResults { get { return IndividulResults.Where(x => x != null).ToList(); } }

        public List<EvaluationEntry> ValidatedResults { get { return EvaluatedResults.Where(x => x.Validation != null).ToList(); } }

        public int Remaing { get; set; }

        public PopulationHistory(int populationSize)
        {
            PopulationSize = populationSize;
            IndividulResults = new List<EvaluationEntry>(populationSize);
            for (int i = 0; i < populationSize; i++)
            {
                IndividulResults.Add(null);
            }
            Remaing = populationSize;
        }

        public bool RateIndividual(Individual ind)
        {
            int individualNumber = ind.NumberOfIndividual;
            var storedEvaluation = IndividulResults[individualNumber];
            if (storedEvaluation == null)
            {
                storedEvaluation = new EvaluationEntry(ind);
                IndividulResults[individualNumber] = storedEvaluation;
                Remaing--;
                return Remaing == 0;
            }
            return false;
        }
    }

    public class EvaluationEntry
    {
        public Individual EvaluatedIndividual { get; set; }

        public EvaluationEntry(Individual evaluatedIndividual)
        {
            EvaluatedIndividual = evaluatedIndividual;
        }

        public List<double> Evaluation
        {
            get { return EvaluatedIndividual.MultiObjectiveFitness; }
            set { EvaluatedIndividual.MultiObjectiveFitness = value; }
        }

        public List<double> Validation
        {
            get { return EvaluatedIndividual.MultiObjectiveValidation; }
            set { EvaluatedIndividual.MultiObjectiveValidation = value; }
        }

        public List<ProgramStatistic> Statistics
        {
            get { return EvaluatedIndividual.Statistics; }
            set { EvaluatedIndividual.Statistics = value; }
        }
    }
}

