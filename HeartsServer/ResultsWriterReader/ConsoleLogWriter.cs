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
		public override void HandleWriteUserConnected(Player player)
		{
			WriteUserConnected(player);
			if (_nextWriter != null)
				_nextWriter.HandleWriteUserConnected(player);
		}
		public override void HandleWriteUserClickedStartGame(Player player)
		{
			WriteUserClickedStartGame(player);
			if (_nextWriter != null)
				_nextWriter.HandleWriteUserClickedStartGame(player);
		}
		public override void HandleWriteStartedGame()
		{
			WriteStartedGame();
			if (_nextWriter != null)
				_nextWriter.HandleWriteStartedGame();
		}
		public override void HandleWritePlayersGotCards(Player[] players)
		{
			WritePlayersGotCards(players);
			if (_nextWriter != null)
				_nextWriter.HandleWritePlayersGotCards(players);
		}
		public override void HandleWritePlayerGaveCardsExchange(Player player, Card[] cards)
		{
			WritePlayerGaveCardsExchange(player, cards);
			if (_nextWriter != null)
				_nextWriter.HandleWritePlayerGaveCardsExchange(player, cards);
		}
		public override void HandleWritePlayerReceivedCardsExchange(Player player, Card[] cards)
		{
			WritePlayerReceivedCardsExchange(player, cards);
			if (_nextWriter != null)
				_nextWriter.HandleWritePlayerReceivedCardsExchange(player, cards);
		}
		public override void HandleWriteTrick(Trick trick)
		{
			WriteTrick(trick);
			if (_nextWriter != null)
				_nextWriter.HandleWriteTrick(trick);
		}

		public override void HandleWritePlayerThrewCard(Player player, Card card)
		{
			WritePlayerThrewCard(player, card);
			if (_nextWriter != null)
				_nextWriter.HandleWritePlayerThrewCard(player, card);
		}
		public override void HandleWritePlayersPointsInRound(Player[] players)
		{
			WritePlayersPointsInRound(players);
			if (_nextWriter != null)
				_nextWriter.HandleWritePlayersPointsInRound(players);
		}
		public override void HandleWritePlayersPointsAfterRound(Player[] players)
		{
			WritePlayersPointsAfterRound(players);
			if (_nextWriter != null)
				_nextWriter.HandleWritePlayersPointsAfterRound(players);
		}
		public override void HandleWritePlacesAfterGame(Player[] players)
		{
			WritePlacesAfterGame(players);
			if (_nextWriter != null)
				_nextWriter.HandleWritePlacesAfterGame(players);
		}
		public override void HandleWritePlayersCards(Player[] players)
		{
			WritePlayersCards(players);
			if (_nextWriter != null)
				_nextWriter.HandleWritePlayersCards(players);
		}
		public override void HandleWriteClientSendMessage(string message)
		{
			throw new NotImplementedException();
		}

		#endregion

		#region ILogWriter methods

		public void WriteUserConnected(Player player)
		{
			Console.WriteLine(GetUserConnectedLog(player));
		}

		public void WriteUserClickedStartGame(Player player)
		{
			Console.WriteLine(GetUserClickedStartGameLog(player));
		}

		public void WriteStartedGame()
		{
			Console.WriteLine(GetStartedGameLog());
		}

		public void WritePlayersGotCards(Player[] players)
		{
			Console.WriteLine(GetPlayersGotCardsLog(players));
		}

		public void WritePlayerGaveCardsExchange(Player player, Card[] cards)
		{
			Console.WriteLine(GetPlayerGaveCardsExchangeLog(player, cards));
		}

		public void WritePlayerReceivedCardsExchange(Player player, Card[] cards)
		{
			Console.WriteLine(GetPlayerReceivedCardsExchangeLog(player, cards));
		}

		public void WritePlayerThrewCard(Player player, Card card)
		{
			Console.WriteLine(GetPlayerThrewCardLog(player, card));
		}

		public void WriteTrick(Trick trick)
		{
			Console.WriteLine(GetTrickLog(trick));
		}

		public void WritePlayersPointsInRound(Player[] players)
		{
			Console.WriteLine(GetPlayersPointsInRoundLog(players));
		}

		public void WritePlayersPointsAfterRound(Player[] players)
		{
			Console.WriteLine(GetPlayersPointsAfterRoundLog(players));
		}

		public void WritePlacesAfterGame(Player[] players)
		{
			Console.WriteLine(GetPlacesAfterGameLog(players));
		}

		public void WritePlayersCards(Player[] players)
		{
			Console.WriteLine(GetPlayersCardsLog(players));
		}

		public void WriteClientSendMessage(Player player, string message)
		{
			Console.WriteLine(GetClientSendMessageLog(player, message));
		}

		#endregion
	}
}
