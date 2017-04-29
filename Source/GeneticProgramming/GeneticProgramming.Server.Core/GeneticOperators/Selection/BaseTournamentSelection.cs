using System;
using System.Collections.Generic;
using GeneticProgramming.Server.Core.GeneticOperators.Comparers;
using GeneticProgramming.Server.Core.GeneticProgramming;
using GeneticProgramming.Server.Core.Helpers;

namespace GeneticProgramming.Server.Core.GeneticOperators.Selection
{
    public abstract class BaseTournamentSelection:IGeneticOperator
    {
        protected  Individual[] TournamentArray;

        /// <summary>
        /// Probability that better individual wins in one vs one fight
        /// </summary>
        double _probabilityOfBetterWinning=0.6;

        /// <summary>
        /// Tournament size
        /// </summary>
        int _tournamentSize;

        /// <summary>
        /// Random number generator
        /// </summary>
        protected Random RandomGenerator = new Random();

        private readonly IComparer<Individual> _comparer;

        /// <summary>
        /// Initializes selection
        /// </summary>
        /// <param name="probabilityOfBetterWinning">Probality of win of better ind</param>
        /// <param name="tournamentSize">Tournament Size</param>
        /// <param name="comparer"></param>
        protected BaseTournamentSelection(double probabilityOfBetterWinning, int tournamentSize, IComparer<Individual> comparer = null)
        {
            _comparer = comparer ?? new FitnessComparer();
            ProbabilityOfBetterWinning = probabilityOfBetterWinning;
            _tournamentSize = tournamentSize;
            TournamentArray= new Individual[_tournamentSize];
        }

        /// <summary>
        /// Probability that better individual wins in one vs one fight
        /// </summary>
        public double ProbabilityOfBetterWinning
        {
            get { return _probabilityOfBetterWinning; }
            set { _probabilityOfBetterWinning = value; }
        }

        /// <summary>
        /// Tournament size
        /// </summary>
        public int TournamentSize
        {
            get { return _tournamentSize; }
            set { _tournamentSize = value; }
        }

        /// <summary>
        /// New tournament
        /// </summary>
        /// <param name="individuals">Individuals in tournamemt</param>
        /// <returns>Selected winning individual</returns>
        public Individual GetCopyOfRandomIndividual(List<Individual> individuals)
        {
            for (int i=0; i < TournamentArray.Length; i++)
            {
                TournamentArray[i] = individuals[RandomGenerator.Next(individuals.Count)];
            }
            Array.Sort(TournamentArray, (ind1, ind2) => -1*_comparer.Compare(ind1, ind2));
            for (int i = 0; i < TournamentArray.Length - 1; i++)
            {
                if (RandomGenerator.NextDouble() < _probabilityOfBetterWinning) return TournamentArray[i].CopyIndividual();
            }
            return TournamentArray[_tournamentSize - 1].CopyIndividual();
        }

        public abstract void ModifyPopolation(Population pop);
    }
}