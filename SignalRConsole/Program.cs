using Microsoft.AspNetCore.SignalR.Client;

namespace SignalRConsole
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			await using var connection = new HubConnectionBuilder().WithUrl("http://localhost:7058/messageshub").Build();
			await connection.StartAsync();


			Console.ReadKey();
		}


	}
}
