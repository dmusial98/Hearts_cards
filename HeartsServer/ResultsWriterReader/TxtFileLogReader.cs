using Hearts_server.GameLogic;
using Hearts_server.GameLogic.Cards;
using Hearts_server.ResultsWriterReader;
using HeartsServer.GameLogic;
using HeartsServer.GameLogic.Consts;
using HeartsServer.GameLogic.History;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace HeartsServer.ResultsWriterReader
{
	public class TxtFileLogReader : IGameReader
	{
		public string FileName { get; set; } = string.Empty;

		public TxtFileLogReader(string fileName) => FileName = fileName;
		public TxtFileLogReader() { }

		public async Task<GameHistory> GetGameHistoryFromFileAsync()
		{
			DirectoryInfo directory = new DirectoryInfo("LogFiles");
			FileInfo file = null;
			if (FileName == string.Empty)
				file = directory.GetFiles().OrderByDescending(f => f.LastWriteTime).First();
			else
				file = directory.GetFiles().FirstOrDefault(f => f.Name == FileName);

			if (file == null)
				throw new FileNotFoundException();

			GameHistory history = new GameHistory();
			string[] linesFromFile = await LoadFromFile(file);
			var listLines = linesFromFile.ToList();

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

			listLines.Remove(startGameLog.First());

			string[] roundStartedLogs = listLines
					.Where(s => s[..3] == LogCodesConsts.ROUND_STARTED_CODE_CONST.PadRight(3, ' '))
					.OrderBy(s => s)
					.ToArray();

			foreach (string roundStartedLog in roundStartedLogs)
			{
				RoundHistory roundHistory = new RoundHistory();

				roundHistory.RoundNumber = Int32.Parse(roundStartedLog.Split("Round ")[1].Split(' ')[0]);

				//Cards before round start
				var playerCardsBeforeGameLogs =
						listLines
						.Where(s => s[..3] == LogCodesConsts.PLAYERS_GOT_CARDS_CODE_CONST.PadRight(3, ' '))
						.OrderBy(s => s)
						.Take(NumbersConsts.PLAYERS_NUMBER_CONST)
						.ToArray();

				foreach (var lineLog in playerCardsBeforeGameLogs)
				{
					roundHistory.PlayerCardsBefore.Add(new PlayerCardsHistory
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
						.OrderBy(s => s)
						.Take(NumbersConsts.PLAYERS_NUMBER_CONST)
						.ToArray();
				string[] cardsReceivedFromExchangeLogs = listLines
						.Where(s => s[..3] == LogCodesConsts.PLAYER_RECEIVED_CARDS_CODE_CONST.PadRight(3, ' '))
						.OrderBy(s => s)
						.Take(NumbersConsts.PLAYERS_NUMBER_CONST)
						.ToArray();

				ExchangeHistory[] exchangeHistoryArray = GetExchangeHistory(cardsGaveToExchangeLogs, cardsReceivedFromExchangeLogs);

				foreach (var line in cardsGaveToExchangeLogs)
					listLines.Remove(line);

				foreach (var line in cardsReceivedFromExchangeLogs)
					listLines.Remove(line);

				string[] playersCardsLog = listLines
						.Where(s => s[..3] == LogCodesConsts.PLAYERS_CARDS_CODE_CONST.PadRight(3, ' '))
						.OrderBy(s => s)
						.Take(NumbersConsts.PLAYERS_NUMBER_CONST)
						.ToArray();

				foreach (var line in playersCardsLog)
				{
					exchangeHistoryArray
							.Where(h => h.IdPlayer == Int32.Parse(line.Split(" ")[7])).ToArray()
							.First().CardsAfter = LoadCardsFromCardsLog(line.Split(" cards: ")[1].Split(", "));

					listLines.Remove(line);
				}

				roundHistory.Exchange = exchangeHistoryArray.ToList();

				//Tricks
				List<string> trickStartedLogs = listLines
						.Where(s => s[..3] == LogCodesConsts.TRICK_STARTED_CODE_CONST.PadRight(3, ' '))
						.OrderBy(s => s)
						.Take(NumbersConsts.TRICKS_NUMBER_IN_ROUND_CONST)
						.ToList();

				List<string> trickSummaryLog = listLines
						.Where(s => s[..3] == LogCodesConsts.TRICK_CODE_CONST.PadRight(3, ' '))
						.OrderBy(s => s)
						.Take(NumbersConsts.TRICKS_NUMBER_IN_ROUND_CONST)
						.ToList();

				var pointsInRound = listLines
						.Where(s => s[..3] == LogCodesConsts.PLAYERS_POINTS_IN_ROUND_CODE_CONST.PadRight(3, ' '))
						.OrderBy(s => s)
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
						WhoStarted = whoStarted,
						WhoWon = Int32.Parse(splittedIds[1].Split(" ")[0]),
					};

					var cardsInTrick = LoadCardsFromCardsLog(trickSummaryLog[0].Split("cards: ")[1].Split(", "));
					int queuePlayerIndex = whoStarted;

					for (int j = 0; j < NumbersConsts.PLAYERS_NUMBER_CONST; j++)
					{
						trickHistory.Queue.Add(new QueueHistory
						{
							PlayerId = queuePlayerIndex,
							Card = cardsInTrick[j],
						});

						if (queuePlayerIndex == 3)
							queuePlayerIndex = 0;
						else
							queuePlayerIndex++;
					}

					for (int j = 0; j < NumbersConsts.PLAYERS_NUMBER_CONST; j++)
					{
						trickHistory.PointsAfterTrick.Add(new PlayerPointsHistory
						{
							PlayerName = pointsInRound.First().Split("player's ")[1].Split(", ")[0],
							PlayerId = Int32.Parse(pointsInRound.First().Split("ID: ")[1].Split(' ')[0]),
							Points = Int32.Parse(pointsInRound.First().Split("): ")[1])
						});

						listLines.Remove(pointsInRound[0]);
						pointsInRound.RemoveAt(0);
					}

					var cardsAfterTrick = listLines
							.Where(s => s[..3] == LogCodesConsts.PLAYERS_CARDS_CODE_CONST.PadRight(3, ' '))
							.OrderBy(s => s)
							.Take(NumbersConsts.PLAYERS_NUMBER_CONST)
							.ToList();

					foreach (var line in cardsAfterTrick)
					{
						trickHistory.PlayerCardsAfterTrick.Add(new PlayerCardsHistory
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
						.OrderBy(s => s)
						.Take(NumbersConsts.PLAYERS_NUMBER_CONST)
						.ToArray();

				foreach (var line in pointsAfterRoundLog)
				{
					roundHistory.PointsAfterRound.Add(new PlayerPointsHistory
					{
						PlayerId = Int32.Parse(pointsAfterRoundLog?.First().Split("ID: ")[1].Split(' ')[0]),
						PlayerName = pointsAfterRoundLog?.First().Split("player's ")[1].Split(',')[0],
						Points = Int32.Parse(pointsAfterRoundLog.First().Split("): ")[1])
					});

					listLines.Remove(line);
				}

				var playerPlacesAfterGameLogs = listLines
					.Where(s => s[..3] == LogCodesConsts.PLAYERS_PLACES_AFTER_GAME_CODE_CONST.PadRight(3, ' '))
					.OrderBy(s => s)
					.Take(NumbersConsts.PLAYERS_NUMBER_CONST)
					.ToArray();



				List<PlayerHistory> playersHistory = new List<PlayerHistory>();
				foreach (var log in playerPlacesAfterGameLogs)
				{
					playersHistory.Add(
						new PlayerHistory
						{
							Id = int.Parse(log.Split("ID: ")[1].Split(' ')[0]),
							Place = int.Parse(log.Split("game: ")[1].Split(' ')[0]),
							Bonuses = int.Parse(log.Split("with ")[1].Split(' ')[0]),
							Name = log.Split("player's ")[1].Split(' ')[0],
						});

					listLines.Remove(log);
				}

				if (playersHistory.Count > 0)
					history.Players = playersHistory;

				history.Rounds.Add(roundHistory);
				listLines.Remove(roundStartedLog);
			}

			return history;
		}

		private async Task<string[]> LoadFromFile(FileInfo file)
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
			List<Card> cards = new();

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
			List<ExchangeHistory> exchangeHistory = new();

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
