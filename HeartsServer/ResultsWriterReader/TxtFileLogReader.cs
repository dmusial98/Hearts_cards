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
			//zaladuj paczke czterech rzuconych kart, wygranie lewy i punkty w rundzie C7 C8 C9
			//zaladuj karty graczy po lewie C12
			//po 13 lewach zaladuj punkty po rundzie C10
			//powtarzaj aż załaduje informację o miejscach C11

			listLines.RemoveAll(IsUnnecessaryLog);

			//Started game
			var startGameLog = listLines.Where(s => s[..3] == "C3 ").ToArray();
			if (startGameLog.Count() != 1)
				throw new FileLoadException($"File {file.Name} doesn't contain exactly one start game message log");

			listLines.Remove(startGameLog.First());

			//Cards before game start
			var playerCardsBeforeGameLogs = listLines.Where(s => s[..3] == "C4 ").Take(4).ToArray();
			List<List<Card>> cardsBeforeStart = new List<List<Card>>();

			foreach (var lineLog in playerCardsBeforeGameLogs)
			{
				string[] splittedCardsLog = lineLog.Split("got cards: ")[1].Split(", ");
				cardsBeforeStart.Add(LoadCardsFromCardsLog(splittedCardsLog));
			}

			foreach (string line in listLines.Where(s => s[..3] == "C4 ").ToArray())
				listLines.Remove(line);

			//Cards exchange
			var cardsGaveToExchangeLogs = listLines.Where(s => s[..3] == "C5 ").ToArray();
			var cardsReceivedToExchangeLogs = listLines.Where(s => s[..3] == "C6 ").ToArray();

			var exchangeHistoryArray = GetExchangeHistory(cardsGaveToExchangeLogs, cardsReceivedToExchangeLogs);
			var playersCardsLog = listLines.Where(s => s[..3] == "C12").Take(4).ToArray();
			
			foreach (var line in playersCardsLog)
			{
				var a = line.Split(" ");
				exchangeHistoryArray
					.Where(h => h.IdPlayer == Int32.Parse(line.Split(" ")[7])).ToArray()
					.First().CardsAfter = LoadCardsFromCardsLog(line.Split(" cards: ")[1].Split(", "));
			}

			foreach (var line in cardsGaveToExchangeLogs)
				listLines.Remove(line);


			return history;
		}

		private async Task<string[]> LoadFromFile(FileInfo file)
		{
			using (var readTask = File.ReadAllLinesAsync(file.FullName))
				return await readTask;
		}

		private static bool IsUnnecessaryLog(string input)
		{
			return input[..3] == "C1 " || input[..3] == "C2 " || input[..3] == "C13";
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
