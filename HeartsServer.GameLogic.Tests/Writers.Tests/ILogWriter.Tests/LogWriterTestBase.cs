using System.Globalization;
using Hearts_server.GameLogic;
using HeartsServer.GameLogic.Shuffle;
using System.Text;
using HeartsServer.ResultsWriterReader;
using Hearts_server.GameLogic.Cards;
using Hearts_server.ResultsWriter;
using System.Numerics;
using HeartsServer.GameLogic.Consts;

namespace HeartsServer.GameLogic.Tests.Writers.Tests.ILogWriter.Tests
{
	public class LogWriterTestBase
	{
		public async Task<string> WriteUserConnected_BaseTest(Hearts_server.ResultsWriter.ILogWriter writer)
		{
			Player player = new("John");
			await writer.WriteUserConnectedAsync(player);

			return String.Concat(LogCodesConsts.USER_CONNECTED_CODE_CONST, " ", DateTime.Now.ToString(LogCodesConsts.POLISH_CULTURE_INFO), ":  player ", player.Name, ", ID: ", player.Id, " connected with server", Consts.LogCodesConsts.NEW_LINE);
		}


		public async Task<string> WriteUserClickedStartGame_BaseTest(Hearts_server.ResultsWriter.ILogWriter writer)
		{
			Player player = new("John");
			await writer.WriteUserClickedStartGameAsync(player);

			return String.Concat(LogCodesConsts.USER_CLICKED_START_CONST, " ", DateTime.Now.ToString(LogCodesConsts.POLISH_CULTURE_INFO), ":  player ", player.Name, ", ID: ", player.Id, " clicked start game", Consts.LogCodesConsts.NEW_LINE);
		}

		public async Task<string> WriteStartGame_BaseTest(Hearts_server.ResultsWriter.ILogWriter writer)
		{
			await writer.WriteStartedGameAsync();

			return String.Concat(LogCodesConsts.GAME_STARTED_CODE_CONST, " ", DateTime.Now.ToString(LogCodesConsts.POLISH_CULTURE_INFO), ": Game started", Consts.LogCodesConsts.NEW_LINE);
		}

		public async Task<string> WritePlayersGotCards_BaseTest(Hearts_server.ResultsWriter.ILogWriter writer)
		{
			var players = new Player[] { new Player("John"), new Player("Paul"), new Player("Michael"), new Player("Joseph") };
			var cards = new StandardRandomShuffleEngine().Shuffle(Game.Instance.GetPackOfCards());

			for (int i = 0; i < NumbersConsts.PLAYERS_NUMBER_CONST; i++)
				players[i].SetCards(cards[i]);

			await writer.WritePlayersGotCardsAsync(players);

			StringBuilder output = new StringBuilder();

			for (int i = 0; i < NumbersConsts.PLAYERS_NUMBER_CONST; i++)
			{
				output.Append(String.Concat(LogCodesConsts.PLAYERS_GOT_CARDS_CODE_CONST, " ", DateTime.Now.ToString(LogCodesConsts.POLISH_CULTURE_INFO), ":  player ", players[i].Name, ", ID: ", players[i].Id, " got cards: "));
				foreach (var card in players[i].OwnCards)
					output.Append(card.ToString()).Append(", ");

				output.Append(Consts.LogCodesConsts.NEW_LINE);
			}

			return output.ToString();
		}

		public async Task<string> WritePlayerGaveCardsExchange_BaseTest(Hearts_server.ResultsWriter.ILogWriter writer)
		{
			Player playerFrom = new Player("John");
			Player playerTo = new Player("Adam");
			Card[] cards =
				new Card[]
				{
					new Card(CardValue.Five, CardColour.Spade),
					new Card(CardValue.Ten, CardColour.Diamond),
					new Card(CardValue.Queen, CardColour.Club)
				};

			await writer.WritePlayerGaveCardsExchangeAsync(playerFrom, playerTo, cards);

			StringBuilder cardsStr = new StringBuilder();
			foreach (var card in cards)
				cardsStr.Append(card.ToString() + ", ");

			return String.Concat(LogCodesConsts.PLAYER_GAVE_CARDS_CODE_CONST, " ", DateTime.Now.ToString(LogCodesConsts.POLISH_CULTURE_INFO), ":  player ", playerFrom.Name, ", ID: ", playerFrom.Id, " gave cards ", cardsStr, " for exchange to ", playerTo.Name, ", ID: ", playerTo.Id, Consts.LogCodesConsts.NEW_LINE);
		}

		public async Task<string> WritePlayerReceivedCardsExchange_BaseTest(Hearts_server.ResultsWriter.ILogWriter writer)
		{
			Player playerFrom = new Player("John");
			Player playerTo = new Player("Adam");
			Card[] cards = new Card[] {
				new Card(CardValue.Five, CardColour.Spade),
				new Card(CardValue.Ten, CardColour.Diamond),
				new Card(CardValue.Queen, CardColour.Club) 
			};

			await writer.WritePlayerReceivedCardsExchangeAsync(playerFrom, playerTo, cards);

			StringBuilder cardsStr = new StringBuilder();
			foreach (var card in cards)
				cardsStr.Append(card.ToString() + ", ");

			return String.Concat(LogCodesConsts.PLAYER_RECEIVED_CARDS_CODE_CONST, " ", DateTime.Now.ToString(LogCodesConsts.POLISH_CULTURE_INFO), ":  player ", playerFrom.Name, ", ID: ", playerFrom.Id, " received cards ", cardsStr, " for exchange from ", playerTo.Name, ", ID: ", playerTo.Id, Consts.LogCodesConsts.NEW_LINE);
		}

		public async Task<string> WritePlayerThrewCard_BaseTest(Hearts_server.ResultsWriter.ILogWriter writer)
		{
			Player player = new Player("John");
			Card card = new Card(CardValue.Ace, CardColour.Heart);

			await writer.WritePlayerThrewCardAsync(player, card);

			return String.Concat(LogCodesConsts.PLAYER_THREW_CARD_CODE_CONST, " ", DateTime.Now.ToString(LogCodesConsts.POLISH_CULTURE_INFO), ":  player ", player.Name, ", ID: ", player.Id.ToString(), " threw card ", card.ToString(), Consts.LogCodesConsts.NEW_LINE);
		}

		public async Task<string> WriteTrick_BaseTest(Hearts_server.ResultsWriter.ILogWriter writer)
		{
			Player playerWhoWonTrick = new("Michael"), playerWhoStarted = new("Adam");
			Card[] cards = new Card[]
			{
				new Card(CardValue.Two, CardColour.Club),
				new Card(CardValue.Eight, CardColour.Club),
				new Card(CardValue.Three, CardColour.Heart),
				new Card(CardValue.Jack, CardColour.Club)
			};

			Trick trick = new Trick(cards, playerWhoWonTrick, playerWhoStarted);

			await writer.WriteTrickAsync(trick);

			StringBuilder cardsStr = new();
			foreach (Card card in cards)
				cardsStr.Append(card.ToString() + ", ");

			return String.Concat(LogCodesConsts.TRICK_CODE_CONST, " ", DateTime.Now.ToString(LogCodesConsts.POLISH_CULTURE_INFO), ":  player ", playerWhoWonTrick.Name, ", ID: ", playerWhoWonTrick.Id.ToString(), " won trick, began " + playerWhoStarted.Name + ", ID: " + playerWhoStarted.Id + " cards: ", cardsStr.ToString(), Consts.LogCodesConsts.NEW_LINE);
		}

		public async Task<string> WritePlayersPointsInRound_BaseTest(Hearts_server.ResultsWriter.ILogWriter writer)
		{
			int roundNumber = 8;
			Player[] players = new Player[]
			{
				new Player("John"),
				new Player("Adam"),
			};

			Trick trick1 = new Trick(
				new Card[]
				{
					new Card(CardValue.Queen, CardColour.Spade),
					new Card(CardValue.Three, CardColour.Club),
					new Card(CardValue.Nine, CardColour.Heart),
					new Card(CardValue.King, CardColour.Diamond)
				},
				players[1],
				players[1]);

			Trick trick2 = new Trick(
				new Card[]
				{
					new Card(CardValue.Nine, CardColour.Club),
					new Card(CardValue.Three, CardColour.Heart),
					new Card(CardValue.Nine, CardColour.Heart),
					new Card(CardValue.King, CardColour.Heart)
				},
				players[0],
				players[0]);

			players[1].AddTrick(trick1);
			players[0].AddTrick(trick2);

			await writer.WritePlayersPointsInRoundAsync(players, roundNumber);

			StringBuilder output = new();
			foreach (Player player in players)
				output.Append(String.Concat(LogCodesConsts.PLAYERS_POINTS_IN_ROUND_CODE_CONST, " ", DateTime.Now.ToString(LogCodesConsts.POLISH_CULTURE_INFO), ":  player's ", player.Name, ", ID: ", player.Id.ToString(), " points in that round (", roundNumber.ToString(), "): ", player.PointsInRound, Consts.LogCodesConsts.NEW_LINE));

			return output.ToString();
		}
		public async Task<string> WritePlayersPointsAfterRound_BaseTest(Hearts_server.ResultsWriter.ILogWriter writer)
		{
			int roundNumber = 5;

			Player[] players = new Player[]
			{
				new Player("John"),
				new Player("Adam"),
			};

			Trick trick1 = new Trick(
				new Card[]
				{
					new Card(CardValue.Queen, CardColour.Spade),
					new Card(CardValue.Three, CardColour.Club),
					new Card(CardValue.Nine, CardColour.Heart),
					new Card(CardValue.King, CardColour.Diamond)
				},
				players[1],
				players[1]);

			Trick trick2 = new Trick(
				new Card[]
				{
					new Card(CardValue.Nine, CardColour.Club),
					new Card(CardValue.Three, CardColour.Heart),
					new Card(CardValue.Nine, CardColour.Heart),
					new Card(CardValue.King, CardColour.Heart)
				},
				players[0],
				players[0]);

			players[1].AddTrick(trick1);
			players[0].AddTrick(trick2);
			players[1].CountPointsAfterRound();
			players[0].CountPointsAfterRound();

			await writer.WritePlayersPointsAfterRoundAsync(players, roundNumber);

			StringBuilder output = new();
			foreach (Player player in players)
				output.Append(String.Concat(LogCodesConsts.PLAYERS_POINTS_AFTER_ROUND_CODE_CONST, " ", DateTime.Now.ToString(LogCodesConsts.POLISH_CULTURE_INFO), ":  player's ", player.Name, ", ID: ", player.Id.ToString(), " points after that round (", roundNumber, "): ", player.Points, Consts.LogCodesConsts.NEW_LINE));

			return output.ToString();
		}

		public async Task<string> WritePlayersPlacesAfterGame_BaseTest(Hearts_server.ResultsWriter.ILogWriter writer)
		{
			int[] places = new int[] { 2, 3, 1, 4 };
			Player[] players = new Player[] 
			{
				new Player("John"),
				new Player("Adam"),
				new Player("David"),
				new Player("Peter")
			};

			for (int i = 0; i < NumbersConsts.PLAYERS_NUMBER_CONST; i++)
				players[i].Place = places[i];

			await writer.WritePlacesAfterGameAsync(players);

			StringBuilder output = new();
			foreach (Player player in players)
				output.Append(String.Concat(LogCodesConsts.PLAYERS_PLACES_AFTER_GAME_CODE_CONST, " ", DateTime.Now.ToString(LogCodesConsts.POLISH_CULTURE_INFO), ":  player's ", player.Name, ", ID: ", player.Id.ToString(), " place in game: ", player.Place, " with ", player.BonusesNumber, " bonuses", Consts.LogCodesConsts.NEW_LINE));

			return output.ToString();
		}

		public async Task<string> WritePlayersCards_BaseTest(Hearts_server.ResultsWriter.ILogWriter writer)
		{
			var shuffleEngine = new StandardRandomShuffleEngine();
			Card[] cards = Game.Instance.GetPackOfCards();
			var cardsForPlayers = shuffleEngine.Shuffle(cards);

			Player[] players = new Player[]
			{
				new Player("John"),
				new Player("Adam"),
				new Player("David"),
				new Player("Peter")
			};

			for (int i = 0; i < NumbersConsts.PLAYERS_NUMBER_CONST; i++)
				players[i].SetCards(cardsForPlayers[i]);

			await writer.WritePlayersCardsAsync(players);

			StringBuilder output = new();
			foreach (Player player in players)
			{
				StringBuilder cardsStr = new();
				foreach (Card card in player.OwnCards)
					cardsStr.Append(card.ToString()).Append(", ");

				output.Append(String.Concat(LogCodesConsts.PLAYERS_CARDS_CODE_CONST, " ", DateTime.Now.ToString(LogCodesConsts.POLISH_CULTURE_INFO), ":  player's ", player.Name, ", ID: ", player.Id.ToString(), " cards: ", cardsStr.ToString(), Consts.LogCodesConsts.NEW_LINE));
			}

			return output.ToString();
		}

		public async Task<string> WriteClientSendMessage_BaseTest(Hearts_server.ResultsWriter.ILogWriter writer)
		{
			Player player = new Player("John");
			string message = "Hello, I sent this message to server";

			await writer.WriteClientSendMessageAsync(player, message);

			return String.Concat(LogCodesConsts.USER_SEND_MESSAGE_CODE_CONST, " ", DateTime.Now.ToString(Consts.LogCodesConsts.POLISH_CULTURE_INFO), ":  player ", player.Name, ", ID: ", player.Id.ToString(), " send message: ", message, LogCodesConsts.NEW_LINE);
		}

		public async Task<string> WriteRoundStarted_BaseTest(Hearts_server.ResultsWriter.ILogWriter writer)
		{
			int roundNumber = 7;

			await writer.WriteStartRoundAsync(roundNumber);

			return String.Concat(LogCodesConsts.ROUND_STARTED_CODE_CONST, " ", DateTime.Now.ToString(LogCodesConsts.POLISH_CULTURE_INFO), ": Round ", roundNumber.ToString(), " started", Consts.LogCodesConsts.NEW_LINE);
		}

		public async Task<string> WriteTrickStarted_BaseTest(Hearts_server.ResultsWriter.ILogWriter writer)
		{
			int trickNumber = 12;
			int roundRumber = 3;

			await writer.WriteStartTrickAsync(trickNumber, roundRumber);

			return String.Concat(LogCodesConsts.TRICK_STARTED_CODE_CONST, " ", DateTime.Now.ToString(LogCodesConsts.POLISH_CULTURE_INFO), ": Trick ", trickNumber.ToString("G"), " in ", roundRumber.ToString(), " round started", Consts.LogCodesConsts.NEW_LINE);
		}


	}


}
