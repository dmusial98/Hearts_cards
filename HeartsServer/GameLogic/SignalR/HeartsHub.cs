using Microsoft.AspNetCore.SignalR;

namespace HeartsServer.GameLogic.SignalR
{
    public class HeartsHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

    }
}
