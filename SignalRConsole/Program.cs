using Microsoft.AspNetCore.SignalR.Client;

namespace SignalRConsole
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			//Ten handler dla linuxa dla uzyskania polaczenia bez specjalnych zabezpieczen, kluczy itd.
			var handler = new HttpClientHandler();
			handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

			await using var connection = new HubConnectionBuilder().WithUrl("wss://localhost:7058/messageshub", 
			//Ten option dla linuxa dla uzyskania polaczenia bez specjalnych zabezpieczen, kluczy itd.
			options => 
			{
        		options.HttpMessageHandlerFactory = _ => handler;
			})
			.WithAutomaticReconnect().Build();
			
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
