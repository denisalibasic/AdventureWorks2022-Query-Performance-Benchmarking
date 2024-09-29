using Microsoft.AspNetCore.SignalR;

namespace AdventureWorksQueryPerformanceApi
{
    public class QueryResultHub : Hub
    {
        public async Task SendQueryResult(string message)
        {
            await Clients.All.SendAsync("ReceiveQueryResult", message);
        }
    }
}
