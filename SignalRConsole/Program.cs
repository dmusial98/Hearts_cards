using Microsoft.AspNetCore.SignalR.Client;

namespace SignalRConsole
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			await using var connection = new HubConnectionBuilder().WithUrl("wss://localhost:7058/messageshub").WithAutomaticReconnect().Build();
			await connection.StartAsync();

			connection.On("ReceiveMessage", (string mess) =>
			{
                Console.WriteLine(mess);
            });

            await connection.SendAsync("WriteText", "Hub received message from client :D");

			Console.ReadKey();
		}


	}
}
