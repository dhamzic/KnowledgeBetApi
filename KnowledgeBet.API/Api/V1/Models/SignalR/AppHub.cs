using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace KnowledgeBet.API.Api.V1.Models.SignalR
{
    public class AppHub : Hub
    {
        /// <summary>
        /// Primanje poruke od klijenta (frontend)
        /// </summary>
        /// <param name="user"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessage(string user, string message)
        {
            //Prosljeđivanje poruke svim klijentima
            await this.Clients.All.SendAsync("broadcastMessageData", user, message);
        }
    }
}
