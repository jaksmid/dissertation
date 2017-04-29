using GeneticProgramming.Core.GPService;
using System.Configuration;

namespace GeneticProgramming.Core.Helpers
{
    /// <summary>
    /// Tools for GPCLient
    /// </summary>
    public class GpClientHelpers
    {
        /// <summary>
        /// Get default GP client
        /// </summary>
        /// <returns>GP client</returns>
        public static GPServiceClient GetClient()
        {
            string endpoint = ConfigurationManager.AppSettings["serverEndpoint"];
            return new GPServiceClient("BasicHttpBinding_IGPService", endpoint);
        }

        /// <summary>
        /// Get GP client with specified endpoint adress
        /// </summary>
        /// <param name="endpoint">Endpoint adress</param>
        /// <returns>GP client</returns>
        public static GPServiceClient GetClient(string endpoint)
        {
            return new GPServiceClient("BasicHttpBinding_IGPService", endpoint);
        }
    }
}
