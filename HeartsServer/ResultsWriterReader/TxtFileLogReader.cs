using Hearts_server.GameLogic;
using Hearts_server.GameLogic.Cards;
using Hearts_server.ResultsWriterReader;
using HeartsServer.GameLogic;
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
			var startGameLog = listLines.Where(s => s[..3] == Consts.GAME_STARTED_CODE_CONST.PadRight(3, ' ')).ToArray();
			if (startGameLog.Count() != 1)
				throw new FileLoadException($"File {file.Name} doesn't contain exactly one start game message log");

			listLines.Remove(startGameLog.First());

			string[] roundStartedLogs = listLines.Where(s => s[..3] == Consts.ROUND_STARTED_CODE_CONST.PadRight(3, ' ')).OrderBy(s => s).ToArray();
			foreach (string roundStartedLog in roundStartedLogs)
			{
				RoundHistory roundHistory = new RoundHistory();

				//Cards before round start
				var playerCardsBeforeGameLogs =
					listLines
					.Where(s => s[..3] == Consts.PLAYERS_GOT_CARDS_CODE_CONST.PadRight(3, ' '))
					.OrderBy(s => s)
					.Take(Consts.PLAYERS_NUMBER_CONST)
					.ToArray();

				List<List<Card>> cardsBeforeStart = new List<List<Card>>();

				foreach (var lineLog in playerCardsBeforeGameLogs)
				{
					string[] splittedCardsLog = lineLog.Split("got cards: ")[1].Split(", ");
					cardsBeforeStart.Add(LoadCardsFromCardsLog(splittedCardsLog));
					listLines.Remove(lineLog);
				}

				//Cards exchange
				string[] cardsGaveToExchangeLogs = listLines
					.Where(s => s[..3] == Consts.PLAYER_GAVE_CARDS_CODE_CONST.PadRight(3, ' '))
					.OrderBy(s => s)
					.Take(Consts.PLAYERS_NUMBER_CONST)
					.ToArray();
				string[] cardsReceivedToExchangeLogs = listLines
					.Where(s => s[..3] == Consts.PLAYER_RECEIVED_CARDS_CODE_CONST.PadRight(3, ' '))
					.OrderBy(s => s)
					.Take(Consts.PLAYERS_NUMBER_CONST)
					.ToArray();

				ExchangeHistory[] exchangeHistoryArray = GetExchangeHistory(cardsGaveToExchangeLogs, cardsReceivedToExchangeLogs);

				foreach (var line in cardsGaveToExchangeLogs)
					listLines.Remove(line);

				foreach (var line in cardsReceivedToExchangeLogs)
					listLines.Remove(line);

				string[] playersCardsLog = listLines
					.Where(s => s[..3] == Consts.PLAYERS_CARDS_CODE_CONST.PadRight(3, ' '))
					.OrderBy(s => s)
					.Take(Consts.PLAYERS_NUMBER_CONST)
					.ToArray();

				foreach (var line in playersCardsLog)
				{
					exchangeHistoryArray
						.Where(h => h.IdPlayer == Int32.Parse(line.Split(" ")[7])).ToArray()
						.First().CardsAfter = LoadCardsFromCardsLog(line.Split(" cards: ")[1].Split(", "));

					listLines.Remove(line);
				}

				//Tricks
				List<string> trickStartedLogs = listLines
					.Where(s => s[..3] == Consts.TRICK_STARTED_CODE_CONST.PadRight(3, ' '))
					.OrderBy(s => s)
					.Take(Consts.TRICKS_NUMBER_IN_ROUND_CONST)
					.ToList();

				List<string> trickSummaryLog = listLines
					.Where(s => s[..3] == Consts.TRICK_CODE_CONST.PadRight(3, ' '))
					.OrderBy(s => s)
					.Take(Consts.TRICKS_NUMBER_IN_ROUND_CONST)
					.ToList();

				foreach (var line in trickStartedLogs)
				{
					TrickHistory trickHistory = new TrickHistory();
					int trickNumber = Int32.Parse(line.Split("Trick ")[1].Split(" ")[0]);


					trickStartedLogs.Remove(line);
					if (trickSummaryLog.Count > 0)
						trickSummaryLog.RemoveAt(0);
				}






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
			return input[..3] == Consts.USER_CONNECTED_CODE_CONST.PadRight(3, ' ') ||
				input[..3] == Consts.USER_CLICKED_START_CONST.PadRight(3, ' ') ||
				input[..3] == Consts.USER_SEND_MESSAGE_CODE_CONST.PadRight(3, ' ');
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
