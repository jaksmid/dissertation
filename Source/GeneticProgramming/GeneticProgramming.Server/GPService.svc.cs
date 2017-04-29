//using System.Collections.Generic;
//using System.Linq;
//using System.ServiceModel;
//using GeneticProgramming.Data.Contracts;
//using GeneticProgramming.Data.Dao;
//using GeneticProgramming.Data.Statistics;
//using GeneticProgramming.Server.Core;
//using GeneticProgramming.Server.Core.DataContracts;
//using GeneticProgramming.Server.Core.GeneticProgramming;
//using GeneticProgramming.Server.Core.Helpers;
//using GeneticProgramming.Server.Hubs.Messages;
//using Microsoft.AspNet.SignalR.Client;

//namespace GeneticProgramming.Server
//{
//    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
//    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
//    public class GPService : IGPService
//    {
//        private readonly DbEntitiesProvider _provider=new DbEntitiesProvider();
//        private GeneticProgrammingExperiment _geneticProgrammingExperiment;
//        private IHubProxy _messageHub;

//        /// <summary>
//        /// Intialization by default initializator
//        /// </summary>
//        public GPService()
//        {
//            _geneticProgrammingExperiment = new GeneticProgrammingExperiment(null);
//            var connection = new HubConnection("http://localhost:3571/");
//            connection.StateChanged+= delegate(StateChange change)
//            {
//                if (change.NewState == ConnectionState.Disconnected)
//                {
//                    connection.Start().Wait();
//                }
//            };
//            _messageHub = connection.CreateHubProxy("GpProgressHub");
//            connection.Start().Wait();
//        }

//        private int popSize = 200;

//        /// <summary>
//        /// Gets basic information about populations
//        /// </summary>
//        /// <returns>Basic info</returns>
//        public BasicInfo GetBasicInfo()
//        {
//            //_messageHub.Invoke("SendMessage", "Hola", "Sukcess");
//            InformClientsAboutProgress();
//            return new BasicInfo { NumberOfPopulations = _geneticProgrammingExperiment.NumberOfPopulations, PopulationSize = popSize };
//        }

//        /// <summary>
//        /// Gets detailed information about one population
//        /// </summary>
//        /// <param name="populationNumber">Population number</param>
//        /// <returns>Population info</returns>
//        public PopulationInfo GetPopulationInfo(int populationNumber)
//        {
//            return _geneticProgrammingExperiment.Populations.Islands[populationNumber].PopInf;
//        }

//        /// <summary>
//        /// Gets non rated individual
//        /// </summary>
//        /// <returns>Non rated individual in envelop representation</returns>
//        public ProgramEnvelope GetIndividual()
//        {
//            var individual = _geneticProgrammingExperiment.GetIndividual();
//            if (individual == null)
//            {
//                //No more rating tasks, either init new population or send finished signal
//            }
//            var envelope = ProgramEnvelopeFactory.CreateProgramEnvelope(individual,
//                _geneticProgrammingExperiment.Experiment.Identificator);
//            return envelope;
//        }

//        /// <summary>
//        /// Get information about specific individual
//        /// </summary>
//        /// <param name="populationNumber">Population number</param>
//        /// <param name="numberOfIndividual">Individual number</param>
//        /// <returns>Individual in envelop</returns>
//        public ProgramEnvelope GetSpecificIndividual(int populationNumber, int numberOfIndividual)
//        {
//            return ProgramEnvelopeFactory.CreateProgramEnvelope(Populations.Islands[populationNumber].Individuals[numberOfIndividual], Experiment.Identificator);
//        }

//        /// <summary>
//        /// Rate individual
//        /// </summary>
//        /// <param name="results">Individual avaluation results</param>
//        public void RateIndividual(IndividualEvaluationResults results)
//        {
//            _messageHub.Invoke("SendMessage", results.EvaluatedBy, results.ToString());
//            if (results.ExperimentIdentifier != _geneticProgrammingExperiment.Experiment.Identificator)
//            {
//                return;
//            }
//            _geneticProgrammingExperiment.RateIndividual(results);
//            InformClientsAboutProgress();
//        }

//        private void InformClientsAboutProgress()
//        {
//            var message = new GpProgressMessage
//            {
//                ExperimentName = Experiment.Identificator,
//                Evaluated = 0,
//                ToEvaluate = 0,
//                Validated = 0,
//                ToValidate = 0,
//                GenerationNumber = _currentGenerationNumber
//            };
//            if (_validationPhase)
//            {
//                message.ToValidate = _toValidate;
//                message.Validated = _toValidate - _validationQueue.Count(x => x.MultiObjectiveValidation == null);
//            }
//            else
//            {
//                message.ToEvaluate = _toEvaluate;
//                message.Evaluated = _toEvaluate - _ratingQueue.Count(x => x.Fitness == null);
//            }
            
//            _messageHub.Invoke("UpdateProgress", message);
//        }

//        private List<MetadataWithResults> _metadataWithResults;

//        public List<MetadataWithResults> GetMetadataWithResults()
//        {
//            if (_metadataWithResults == null)
//            {
//                _metadataWithResults= _provider.MetadataWithResults.ToList();
//            }
//            return _metadataWithResults;
//        }

//        private List<BestResult> _bestResultContracts;

//        public List<BestResult> GetBestResults()
//        {
//            if (_bestResultContracts == null)
//            {
//                _bestResultContracts = _provider.BestResults.ToList();
//            }
//            return _bestResultContracts;
//        }
//    }
//}
