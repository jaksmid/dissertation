using GeneticProgramming.Server.Hubs.Messages;
using Microsoft.AspNet.SignalR;

namespace GeneticProgramming.Server.Hubs
{
    public class GpProgressHub : Hub
    {
        public void SendMessage(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.broadcastMessage(name, message);
        }

        public void UpdateProgress(GpProgressMessage message)
        {
            Clients.All.updateProgress(message);
        }
    }
}