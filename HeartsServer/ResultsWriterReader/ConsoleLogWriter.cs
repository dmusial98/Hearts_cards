using Hearts_server.GameLogic;
using Hearts_server.GameLogic.Cards;
using Hearts_server.ResultsWriter;
using Hearts_server.ResultsWriterReader;
using System.Diagnostics;
using System.Numerics;
using System.Text;

namespace HeartsServer.ResultsWriterReader
{
	//TODO: pomyslec czy nie przeniesc handlowych metod do BaseGameWriter
	//TODO: pomyslec czy nie sparametryzowac handlowych metod do jednej
	public class ConsoleLogWriter : BaseGameWriter, ILogWriter
	{
		#region Handles
		public override async Task HandleWriteUserConnectedAsync(Player player)
		{
			await WriteUserConnectedAsync(player);
			if (_nextWriter != null)
				await _nextWriter.HandleWriteUserConnectedAsync(player);
		}
		public override async Task HandleWriteUserClickedStartGameAsync(Player player)
		{
			await WriteUserClickedStartGameAsync(player);
			if (_nextWriter != null)
				await _nextWriter.HandleWriteUserClickedStartGameAsync(player);
		}
		public override async Task HandleWriteStartedGameAsync()
		{
			await WriteStartedGameAsync();
			if (_nextWriter != null)
				await _nextWriter.HandleWriteStartedGameAsync();
		}
		public override async Task HandleWritePlayersGotCardsAsync(Player[] players)
		{
			await WritePlayersGotCardsAsync(players);
			if (_nextWriter != null)
				await _nextWriter.HandleWritePlayersGotCardsAsync(players);
		}
		public override async Task HandleWritePlayerGaveCardsExchangeAsync(Player playerFrom, Player playerTo, Card[] cards)
		{
			await WritePlayerGaveCardsExchangeAsync(playerFrom, playerTo, cards);
			if (_nextWriter != null)
				await _nextWriter.HandleWritePlayerGaveCardsExchangeAsync(playerFrom, playerTo, cards);
		}
		public override async Task HandleWritePlayerReceivedCardsExchangeAsync(Player playerFrom, Player playerTo, Card[] cards)
		{
			await WritePlayerReceivedCardsExchangeAsync(playerFrom, playerTo, cards);
			if (_nextWriter != null)
				await _nextWriter.HandleWritePlayerReceivedCardsExchangeAsync(playerFrom, playerTo, cards);
		}
		public override async Task HandleWriteTrickAsync(Trick trick)
		{
			await WriteTrickAsync(trick);
			if (_nextWriter != null)
				await _nextWriter.HandleWriteTrickAsync(trick);
		}

		public override async Task HandleWritePlayerThrewCardAsync(Player player, Card card)
		{
			await WritePlayerThrewCardAsync(player, card);
			if (_nextWriter != null)
				await _nextWriter.HandleWritePlayerThrewCardAsync(player, card);
		}
		public override async Task HandleWritePlayersPointsInRoundAsync(Player[] players)
		{
			await WritePlayersPointsInRoundAsync(players);
			if (_nextWriter != null)
				await _nextWriter.HandleWritePlayersPointsInRoundAsync(players);
		}
		public override async Task HandleWritePlayersPointsAfterRoundAsync(Player[] players, int roundNumber)
		{
			await WritePlayersPointsAfterRoundAsync(players, roundNumber);
			if (_nextWriter != null)
				await _nextWriter.HandleWritePlayersPointsAfterRoundAsync(players, roundNumber);
		}
		public override async Task HandleWritePlacesAfterGameAsync(Player[] players)
		{
			await WritePlacesAfterGameAsync(players);
			if (_nextWriter != null)
				await _nextWriter.HandleWritePlacesAfterGameAsync(players);
		}
		public override async Task HandleWritePlayersCardsAsync(Player[] players)
		{
			await WritePlayersCardsAsync(players);
			if (_nextWriter != null)
				await _nextWriter.HandleWritePlayersCardsAsync(players);
		}
		public override async Task HandleWriteClientSendMessageAsync(Player player, string message)
		{
			await WriteClientSendMessageAsync(player, message);
			if (_nextWriter != null)
				await _nextWriter.HandleWriteClientSendMessageAsync(player, message);
		}

		#endregion

		#region ILogWriter methods

		public async Task WriteUserConnectedAsync(Player player)
		{
			Console.WriteLine(GetUserConnectedLog(player));
		}

		public async Task WriteUserClickedStartGameAsync(Player player)
		{
			Console.WriteLine(GetUserClickedStartGameLog(player));
		}

		public async Task WriteStartedGameAsync()
		{
			Console.WriteLine(GetStartedGameLog());
		}

		public async Task WritePlayersGotCardsAsync(Player[] players)
		{
			Console.Write(GetPlayersGotCardsLog(players));
		}

		public async Task WritePlayerGaveCardsExchangeAsync(Player playerFrom, Player playerTo, Card[] cards)
		{
			Console.WriteLine(GetPlayerGaveCardsExchangeLog(playerFrom, playerTo, cards));
		}

		public async Task WritePlayerReceivedCardsExchangeAsync(Player playerFrom, Player playerTo, Card[] cards)
		{
			Console.WriteLine(GetPlayerReceivedCardsExchangeLog(playerFrom, playerTo, cards));
		}

		public async Task WritePlayerThrewCardAsync(Player player, Card card)
		{
			Console.WriteLine(GetPlayerThrewCardLog(player, card));
		}

		public async Task WriteTrickAsync(Trick trick)
		{
			Console.WriteLine(GetTrickLog(trick));
		}

		public async Task WritePlayersPointsInRoundAsync(Player[] players)
		{
			Console.Write(GetPlayersPointsInRoundLog(players));
		}

		public async Task WritePlayersPointsAfterRoundAsync(Player[] players, int roundNumber)
		{
			Console.Write(GetPlayersPointsAfterRoundLog(players, roundNumber));
		}

		public async Task WritePlacesAfterGameAsync(Player[] players)
		{
			Console.Write(GetPlacesAfterGameLog(players));
		}

		public async Task WritePlayersCardsAsync(Player[] players)
		{
			Console.Write(GetPlayersCardsLog(players));
		}

		public async Task WriteClientSendMessageAsync(Player player, string message)
		{
			Console.WriteLine(GetClientSendMessageLog(player, message));
		}

		#endregion
	}
}
