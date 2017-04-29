using System;
using System.Collections.Generic;
using GeneticProgramming.Data.Statistics;

namespace GeneticProgramming.Server.Core.Serialization
{
    /// <summary>
    /// Serializable population info representation
    /// </summary>
    [Serializable]
    public class PopulationInfoSerializable
    {
        //TODO Best individual

        /// <summary>
        /// Generation number
        /// </summary>
        public uint GenerationNumber;
        /// <summary>
        /// Population sizes
        /// </summary>
        public int PopSize;

        /// <summary>
        /// Fitness of best individual
        /// </summary>
        public double BestIndividualFitness;

        /// <summary>
        /// Average fitness
        /// </summary>
        public double AverageFitness;

        /// <summary>
        /// Average number of nodes
        /// </summary>
        public double AverageNumberOFNodes;

        /// <summary>
        /// Average depth
        /// </summary>
        public double AverageDepth;

        /// <summary>
        /// Average width
        /// </summary>
        public double AverageWidth;

        /// <summary>
        /// Fitness distribution
        /// </summary>
        public FitnessDIstributionCouple[] FitnessDistribution;

        /// <summary>
        /// Previous generation info
        /// </summary>
        PopulationInfoSerializable Parent = null;

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="popInf">Non serializable pop info representation</param>
        public PopulationInfoSerializable(PopulationInfo popInf)
        {
            GenerationNumber = popInf.GenerationNumber;
            PopSize = popInf.PopSize;
            BestIndividualFitness = popInf.BestIndividualFitness;
            AverageFitness = popInf.AverageFitness;
            //TODO: fix after fixing serialization
            //AverageDepth = popInf.AverageDepth;
            //AverageNumberOfNodes = popInf.AverageNumberOfNodes;
            //AverageWidth = popInf.AverageWidth;
            if (popInf.Parent != null) Parent = new PopulationInfoSerializable(popInf.Parent);
            FitnessDistribution = new FitnessDIstributionCouple[popInf.FitnessDistribution.Count];
            int i = 0;
            foreach (var kvp in popInf.FitnessDistribution)
            {
                FitnessDistribution[i] = new FitnessDIstributionCouple(kvp);
                i++;
            }
        }

        /// <summary>
        /// GEt non serializable representation
        /// </summary>
        /// <returns>Non serializable representation</returns>
        public PopulationInfo GetPopulationInfo()
        {
            var toReturn = new PopulationInfo();
            toReturn.AverageFitness = AverageFitness;
            //TODO: fix after fixing serialization
            //toReturn.AverageWidth = AverageWidth;
            //toReturn.AverageNumberOfNodes = AverageNumberOfNodes;
            //toReturn.AverageDepth = AverageDepth;
            toReturn.BestIndividualFitness = BestIndividualFitness;
            toReturn.GenerationNumber = GenerationNumber;
            if (Parent != null) toReturn.Parent = Parent.GetPopulationInfo();
            toReturn.PopSize = PopSize;
            toReturn.FitnessDistribution = new Dictionary<double, int>();
            foreach (var couple in FitnessDistribution)
            {
                toReturn.FitnessDistribution.Add(couple.Key, couple.Value);
            }
            return toReturn;
        }
    }

    /// <summary>
    /// Serializable representation of fitness distribution couple
    /// </summary>
    [Serializable]
    public class FitnessDIstributionCouple
    {
        public double Key;
        public int Value;

        public FitnessDIstributionCouple(KeyValuePair<double, int> kvp)
        {
            Key = kvp.Key;
            Value = kvp.Value;
        }
    }
}