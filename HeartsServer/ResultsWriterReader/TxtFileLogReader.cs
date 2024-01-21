using Hearts_server.GameLogic;
using Hearts_server.GameLogic.Cards;
using Hearts_server.ResultsWriterReader;
using HeartsServer.GameLogic;
using HeartsServer.GameLogic.Consts;
using HeartsServer.GameLogic.History;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Globalization;

namespace HeartsServer.ResultsWriterReader
{
	public class TxtFileLogReader : GameFileReader, IGameReader
	{
		public TxtFileLogReader(string fileName) : base(fileName) { }
		public TxtFileLogReader() : base() { }


		//TODO: ogarnac punkty graczy po kazdej z runk w glownej historii gry
		public override async Task<GameHistory> GetGameHistoryAsync()
		{
			GameHistory history = new GameHistory();
			var file = GetFile();
			var listLines = (await LoadFromFileAsync(file)).ToList();

			//usun niepotrzebne logi +
			//zaladuj start gry C3 + 
			//zaladuj rozdanie kart C4 +
			//zaladuj wymianki i karty po wymiance C5 C6 C12 +
			//zaladuj log o rozpoczeciu rundy, rozpoczeciu lewy, paczke czterech rzuconych kart, wygranie lewy i punkty w rundzie C7 C8 C9
			//zaladuj karty graczy po lewie C12
			//po 13 lewach zaladuj punkty po rundzie C10
			//powtarzaj aż załaduje informację o miejscach C11

			listLines.RemoveAll(IsUnnecessaryLog);

			//Started game
			var startGameLog = listLines
					.Where(s => s[..3] == LogCodesConsts.GAME_STARTED_CODE_CONST.PadRight(3, ' '))
					.ToArray();
			if (startGameLog.Count() != 1)
				throw new FileLoadException($"File {file.Name} doesn't contain exactly one start game message log");

			history.StartTime = DateTime.ParseExact(startGameLog.First().Substring(3, 19), "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture);

			listLines.Remove(startGameLog.First());

			string[] roundStartedLogs = listLines
					.Where(s => s[..3] == LogCodesConsts.ROUND_STARTED_CODE_CONST.PadRight(3, ' '))
					//.OrderBy(s => s)
					.ToArray();



			foreach (string roundStartedLog in roundStartedLogs)
			{
				RoundHistory roundHistory = new RoundHistory();

				roundHistory.RoundNumber = Int32.Parse(roundStartedLog.Split("Round ")[1].Split(' ')[0]);

				//Cards before round start
				var playerCardsBeforeGameLogs =
						listLines
						.Where(s => s[..3] == LogCodesConsts.PLAYERS_GOT_CARDS_CODE_CONST.PadRight(3, ' '))
						//.OrderBy(s => s)
						.Take(NumbersConsts.PLAYERS_NUMBER_CONST)
						.ToArray();

				if(history.Players == null || history.Players.Count == 0)
                    history.Players = new();

                foreach (var lineLog in playerCardsBeforeGameLogs)
				{
                    if (history.Players == null || history.Players.Count <  NumbersConsts.PLAYERS_NUMBER_CONST)
                    {
						var a = lineLog.Split("player ")[1];
						var b = a.Split(", ID")[0];
						var c = lineLog.Split("player ")[1].Split(", ID")[0];

                        history.Players.Add(new PlayerHistory
						{
							PlayerId = Int32.Parse(lineLog.Split("ID: ")[1].Split(' ')[0]),
                            Name = lineLog.Split("player ")[1].Split(", ID")[0],
                        });

                    }
                    roundHistory.PlayerCardsBeforeExchange.Add(new PlayerCardsHistory
					{
						PlayerId = Int32.Parse(lineLog.Split("ID: ")[1].Split(' ')[0]),
						PlayerName = lineLog.Split("player ")[1].Split(',')[0],
						Cards = LoadCardsFromCardsLog(lineLog.Split("got cards: ")[1].Split(", "))
					});

					listLines.Remove(lineLog);
				}

				//Cards exchange
				string[] cardsGaveToExchangeLogs = listLines
						.Where(s => s[..3] == LogCodesConsts.PLAYER_GAVE_CARDS_CODE_CONST.PadRight(3, ' '))
						//.OrderBy(s => s)
						.Take(NumbersConsts.PLAYERS_NUMBER_CONST)
						.ToArray();
				string[] cardsReceivedFromExchangeLogs = listLines
						.Where(s => s[..3] == LogCodesConsts.PLAYER_RECEIVED_CARDS_CODE_CONST.PadRight(3, ' '))
						//.OrderBy(s => s)
						.Take(NumbersConsts.PLAYERS_NUMBER_CONST)
						.ToArray();

				ExchangeHistory[] exchangeHistoryArray = GetExchangeHistory(cardsGaveToExchangeLogs, cardsReceivedFromExchangeLogs);

				foreach (var line in cardsGaveToExchangeLogs)
					listLines.Remove(line);

				foreach (var line in cardsReceivedFromExchangeLogs)
					listLines.Remove(line);

				string[] playersCardsLog = listLines
						.Where(s => s[..3] == LogCodesConsts.PLAYERS_CARDS_CODE_CONST.PadRight(3, ' '))
						//.OrderBy(s => s)
						.Take(NumbersConsts.PLAYERS_NUMBER_CONST)
						.ToArray();

				
				foreach (var line in playersCardsLog)
				{
					int playerIndex_ = Int32.Parse(line.Split("ID: ")[1].Substring(0, 1));
					roundHistory.PlayerCardsAfterExchange.Add(new PlayerCardsHistory
					{
						PlayerId = playerIndex_,
						PlayerName = history.Players.First(p => p.PlayerId == playerIndex_).Name,
                        Cards = LoadCardsFromCardsLog(line.Split(" cards: ")[1].Split(", ")),
                    });
					
                    listLines.Remove(line);
				}

				roundHistory.Exchange = exchangeHistoryArray.ToList();


				//Tricks
				List<string> trickStartedLogs = listLines
						.Where(s => s[..3] == LogCodesConsts.TRICK_STARTED_CODE_CONST.PadRight(3, ' '))
						//.OrderBy(s => s)
						.Take(NumbersConsts.TRICKS_NUMBER_IN_ROUND_CONST)
						.ToList();

				List<string> trickSummaryLog = listLines
						.Where(s => s[..3] == LogCodesConsts.TRICK_CODE_CONST.PadRight(3, ' '))
						//.OrderBy(s => s)
						.Take(NumbersConsts.TRICKS_NUMBER_IN_ROUND_CONST)
						.ToList();

				var pointsInRound = listLines
						.Where(s => s[..3] == LogCodesConsts.PLAYERS_POINTS_IN_ROUND_CODE_CONST.PadRight(3, ' '))
						//.OrderBy(s => s)
						.Take(NumbersConsts.TRICKS_NUMBER_IN_ROUND_CONST * NumbersConsts.PLAYERS_NUMBER_CONST)
						.ToList();

				//for every trick in round
				for (int i = 0; i < NumbersConsts.TRICKS_NUMBER_IN_ROUND_CONST; i++)
				{
					if (trickStartedLogs.Count <= 0 || trickSummaryLog.Count <= 0)
						break;

					var splittedIds = trickSummaryLog[0].Split("ID: ");
					int whoStarted = Int32.Parse(splittedIds[2].Split(" ")[0]);

					TrickHistory trickHistory = new TrickHistory
					{
						TrickNumber = Int32.Parse(trickStartedLogs.First().Split("Trick ")[1].Split(" ")[0]),
						WhoStartedId = whoStarted,
						WhoWonId = Int32.Parse(splittedIds[1].Split(" ")[0]),
					};

					var cardsInTrick = LoadCardsFromCardsLog(trickSummaryLog[0].Split("cards: ")[1].Split(", "));
					int queuePlayerIndex = 1;

					for (int j = 0; j < NumbersConsts.PLAYERS_NUMBER_CONST; j++)
					{
						trickHistory.Queue.Add(new QueueHistory
						{
							PlayerId = queuePlayerIndex,
							Card = cardsInTrick[j],
						});

						queuePlayerIndex++;
					}

					for (int j = 0; j < NumbersConsts.PLAYERS_NUMBER_CONST; j++)
					{
						trickHistory.PointsAfterTrick.Add(new PlayerHistory
						{
							Name = pointsInRound.First().Split("player's ")[1].Split(", ")[0],
							PlayerId = Int32.Parse(pointsInRound.First().Split("ID: ")[1].Split(' ')[0]),
							PointsInRound = Int32.Parse(pointsInRound.First().Split("): ")[1])
						});

						listLines.Remove(pointsInRound[0]);
						pointsInRound.RemoveAt(0);
					}

					var cardsAfterTrick = listLines
							.Where(s => s[..3] == LogCodesConsts.PLAYERS_CARDS_CODE_CONST.PadRight(3, ' '))
							//.OrderBy(s => s)
							.Take(NumbersConsts.PLAYERS_NUMBER_CONST)
							.ToList();

					foreach (var line in cardsAfterTrick)
					{
						trickHistory.PlayerCardsBeforeTrick.Add(new PlayerCardsHistory
						{
							PlayerId = Int32.Parse(line.Split("ID: ")[1].Split(' ')[0]),
							PlayerName = line.Split("player's ")[1].Split(',')[0],
							Cards = LoadCardsFromCardsLog(line.Split("cards: ")[1].Split(", ")),
						});

						listLines.Remove(line);
					}

					roundHistory.Tricks.Add(trickHistory);

					listLines.Remove(trickStartedLogs[0]);
					listLines.Remove(trickSummaryLog[0]);
					trickStartedLogs.RemoveAt(0);
					trickSummaryLog.RemoveAt(0);
				}

				//Add pointsAfterRound
				var pointsAfterRoundLog = listLines
						.Where(s => s[..3] == LogCodesConsts.PLAYERS_POINTS_AFTER_ROUND_CODE_CONST.PadRight(3, ' '))
						//.OrderBy(s => s)
						.Take(NumbersConsts.PLAYERS_NUMBER_CONST)
						.ToArray();

				if (roundHistory.PlayersAfterRound == null)
					roundHistory.PlayersAfterRound = [];

				int[] pointsAfterRound = new int[4];
				int playerIndex = 0;

				foreach (var line in pointsAfterRoundLog)
				{
					pointsAfterRound[playerIndex] = Int32.Parse(line.Split("): ")[1]);

					//roundHistory.PlayersAfterRound.Add(new PlayerHistory
					//{
					//	PlayerId = Int32.Parse(pointsAfterRoundLog[playerIndex]?.Split("ID: ")[1].Split(' ')[0]),
					//	Name = pointsAfterRoundLog[playerIndex]?.Split("player's ")[1].Split(',')[0],
					//	PointsInGame = pointsAfterRound[playerIndex]
					//});

					listLines.Remove(line);
					playerIndex++;
				}

				var playerPlacesAfterGameLogs = listLines
					.Where(s => s[..3] == LogCodesConsts.PLAYERS_PLACES_AFTER_ROUND_CODE_CONST.PadRight(3, ' '))
					//.OrderBy(s => s)
					.Take(NumbersConsts.PLAYERS_NUMBER_CONST)
					.ToArray();



				List<PlayerHistory> playersHistory = new List<PlayerHistory>();
				int pl_index = 0;
				foreach (var log in playerPlacesAfterGameLogs)
				{
					var plHistoryObject = new PlayerHistory
                    {
                        PlayerId = int.Parse(log.Split("ID: ")[1].Split(' ')[0]),
                        Place = int.Parse(log.Split("game: ")[1].Split(' ')[0]),
                        Bonuses = int.Parse(log.Split("with ")[1].Split(' ')[0]),
                        Name = log.Split("player's ")[1].Split(", ")[0],
                        PointsInGame = pointsAfterRound[pl_index]
                    };

                    playersHistory.Add(plHistoryObject);
					roundHistory.PlayersAfterRound.Add(plHistoryObject);

					listLines.Remove(log);
					pl_index++;
				}

				if (playersHistory.Count > 0)
					history.Players = playersHistory;

				history.Rounds.Add(roundHistory);
				listLines.Remove(roundStartedLog);
			} //round

			return history;
		}

		private async Task<string[]> LoadFromFileAsync(FileInfo file)
		{
            using (var readTask = File.ReadAllLinesAsync(file.FullName))
                return await readTask;
        }

		private static bool IsUnnecessaryLog(string input)
		{
			return input[..3] == LogCodesConsts.USER_CONNECTED_CODE_CONST.PadRight(3, ' ') ||
					input[..3] == LogCodesConsts.USER_CLICKED_START_CONST.PadRight(3, ' ') ||
					input[..3] == LogCodesConsts.USER_SEND_MESSAGE_CODE_CONST.PadRight(3, ' ');
		}

		private List<Card> LoadCardsFromCardsLog(string[] input)
		{
			List<Card> cards = [];

			foreach (var str in input)
			{
				if (String.IsNullOrEmpty(str))
					continue;

				var cardStr = str.Split(' ');

				CardValue value = (CardValue)Enum.Parse(typeof(CardValue), cardStr[0]);
				CardColour colour = (CardColour)Enum.Parse(typeof(CardColour), cardStr[1]);

				var card = Game.Instance.GetCard(value, colour);
				if (card != null)
					cards.Add(card);
			}

			return cards;
		}

		private ExchangeHistory[] GetExchangeHistory(string[] gaveLogs, string[] receivedLogs)
		{
			List<ExchangeHistory> exchangeHistory = [];

			foreach (var gaveLog in gaveLogs)
			{
				var splitted = gaveLog.Split(' ');

				exchangeHistory.Add(new ExchangeHistory
				{
					IdPlayer = Int32.Parse(splitted[7]),
					FromWho = Int32.Parse(gaveLogs.Where(s => s.Split(' ')[22] == splitted[7]).ToArray().First().Split(' ')[7]),
					ToWho = Int32.Parse(splitted[22]),
					Gave = LoadCardsFromCardsLog(gaveLog.Split("gave cards ")[1].Split(", ")[..3]),
					Received = LoadCardsFromCardsLog(gaveLogs.Where(s => s.Split(' ')[22] == splitted[7]).ToArray().First().Split("gave cards ")[1].Split(", ")[..3]),
				});
			}

			return exchangeHistory.ToArray();
		}

	}
}
