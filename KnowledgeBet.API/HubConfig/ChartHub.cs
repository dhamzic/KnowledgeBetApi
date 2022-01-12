using KnowledgeBet.API.Models;
using Microsoft.AspNetCore.SignalR;

namespace KnowledgeBet.API.HubConfig
{
    public class ChartHub : Hub
    {
        public async Task BroadcastChartData(List<ChartModel> data) => await Clients.All.SendAsync("broadcastData", data);
    }
}
