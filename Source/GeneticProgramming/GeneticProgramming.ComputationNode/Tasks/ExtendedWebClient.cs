using System;
using System.Net;

namespace GeneticProgramming.ComputationNode.Tasks
{
    public class ExtendedWebClient : WebClient
    {

        private int timeout;
        public int Timeout
        {
            get
            {
                return timeout;
            }
            set
            {
                timeout = value;
            }
        }
        public ExtendedWebClient(Uri address)
        {
            this.timeout = 1000000;//In Milli seconds
            var objWebClient = GetWebRequest(address);
        }
        public ExtendedWebClient()
        {
            this.timeout = 1000000;//In Milli seconds
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var objWebRequest= base.GetWebRequest(address);
            objWebRequest.Timeout = this.timeout;
            return objWebRequest;
        }
    }
}
