using Microsoft.AspNetCore.SignalR;

namespace HeartsServer.SignalR
{
    public class HeartsHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("ReceiveMessage", $"{Context.ConnectionId} has joined");
        }   

        public async Task WriteText(string text)
        {
            await Console.Out.WriteLineAsync(text);
        }

    }
}
