using System.Collections.Generic;
using System.ServiceModel;
using GeneticProgramming.Data.Contracts;
using GeneticProgramming.Data.Statistics;

namespace GeneticProgramming.Server
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IGPService
    {

        /// <summary>
        /// Gets detailed information about one population
        /// </summary>
        /// <param name="populationNumber">Population number</param>
        /// <returns>Population info</returns>
        [OperationContract]
        PopulationInfo GetPopulationInfo(int populationNumber);

        /// <summary>
        /// Load computation progress from file
        /// </summary>
        /// <param name="path">Path to file</param>
        [OperationContract]
        void LoadPopulation(string path);

        /// <summary>
        /// Init populations
        /// </summary>
        /// <param name="initializatorName">Intializator name</param>
        /// <returns>If success</returns>
        [OperationContract]
        bool Init(string initializatorName);

        /// <summary>
        /// Gets non rated individual
        /// </summary>
        /// <returns>Non rated individual in envelop representation</returns>
        [OperationContract]
        ProgramEnvelope GetIndividual();

        /// <summary>
        /// Get information about specific individual
        /// </summary>
        /// <param name="populationNumber">Population number</param>
        /// <param name="numberOfIndividual">Individual number</param>
        /// <returns>Individual in envelop</returns>
        [OperationContract]
        ProgramEnvelope GetSpecificIndividual(int populationNumber, int numberOfIndividual);

        /// <summary>
        /// Gets basic information about populatins
        /// </summary>
        /// <returns>Basic info</returns>
        [OperationContract]
        BasicInfo GetBasicInfo();

        /// <summary>
        /// Gets fitness info about one population
        /// </summary>
        /// <param name="populationNumber">Population number</param>
        /// <returns>Fitness info</returns>
        [OperationContract]
        FitnessInfo GetFitnessInfo(int populationNumber);

        /// <summary>
        /// Rate individual
        /// </summary>
        /// <param name="results">Individual avaluation results</param>
        [OperationContract]
        void RateIndividual(IndividualEvaluationResults results);

        [OperationContract]
        List<MetadataWithResults> GetMetadataWithResults();

        [OperationContract]
        List<BestResult> GetBestResults();

    }
}
