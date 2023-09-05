using System.Globalization;
using Hearts_server.GameLogic.Cards;
using Hearts_server.GameLogic;
using Hearts_server.ResultsWriter;
using Hearts_server.ResultsWriterReader;
using System.Numerics;
using HeartsServer.GameLogic.Consts;

namespace HeartsServer.ResultsWriterReader
{
	public class TxtFileLogWriter : BaseGameWriter, ILogWriter
	{
		private string _pathToFile;
		public TxtFileLogWriter()
		{
			_pathToFile = $"LogFiles/{DateTime.Now.ToString(new CultureInfo("pl-PL"))}_logs.txt".Replace(" ", "_").Replace(":", "_");
		}

		#region ILogWriter methods

		public async Task WriteUserConnectedAsync(Player player)
		{
			string output = GetUserConnectedLog(player);
			output = output + (LogCodesConsts.NEW_LINE);
			using (Task writeTask = File.AppendAllTextAsync(_pathToFile, output))
				await writeTask;
		}

		public async Task WriteUserClickedStartGameAsync(Player player)
		{
			string output = GetUserClickedStartGameLog(player);
			output = output + (LogCodesConsts.NEW_LINE);
			using (Task writeTask = File.AppendAllTextAsync(_pathToFile, output))
				await writeTask;
		}

		public async Task WriteStartedGameAsync()
		{
			string output = GetStartedGameLog();
			output = output + (LogCodesConsts.NEW_LINE);
			using (Task writeTask = File.AppendAllTextAsync(_pathToFile, output))
				await writeTask;
		}

		public async Task WritePlayersGotCardsAsync(Player[] players)
		{
			string output = GetPlayersGotCardsLog(players);
	
			using (Task writeTask = File.AppendAllTextAsync(_pathToFile, output))
				await writeTask;
		}

		public async Task WritePlayerGaveCardsExchangeAsync(Player playerFrom, Player playerTo, Card[] cards)
		{
			string output = GetPlayerGaveCardsExchangeLog(playerFrom, playerTo, cards);
			output = output + (LogCodesConsts.NEW_LINE);
			using (Task writeTask = File.AppendAllTextAsync(_pathToFile, output))
				await writeTask;
		}

		public async Task WritePlayerReceivedCardsExchangeAsync(Player playerFrom, Player playerTo, Card[] cards)
		{
			string output = GetPlayerReceivedCardsExchangeLog(playerFrom, playerTo, cards);
			output = output + (LogCodesConsts.NEW_LINE);
			using (Task writeTask = File.AppendAllTextAsync(_pathToFile, output))
				await writeTask;
		}

		public async Task WritePlayerThrewCardAsync(Player player, Card card)
		{
			string output = GetPlayerThrewCardLog(player, card);
			output = output + (LogCodesConsts.NEW_LINE);
			using (Task writeTask = File.AppendAllTextAsync(_pathToFile, output))
				await writeTask;
		}

		public async Task WriteTrickAsync(Trick trick)
		{
			string output = GetTrickLog(trick);
			output = output + (LogCodesConsts.NEW_LINE);
			using (Task writeTask = File.AppendAllTextAsync(_pathToFile, output))
				await writeTask;
		}

		public async Task WritePlayersPointsInRoundAsync(Player[] players, int roundNumber)
		{
			string output = GetPlayersPointsInRoundLog(players, roundNumber);
			using (Task writeTask = File.AppendAllTextAsync(_pathToFile, output))
				await writeTask;
		}

		public async Task WritePlayersPointsAfterRoundAsync(Player[] players, int roundNumber)
		{
			string output = GetPlayersPointsAfterRoundLog(players, roundNumber);
			using (Task writeTask = File.AppendAllTextAsync(_pathToFile, output))
				await writeTask;
		}

		public async Task WritePlacesAfterGameAsync(Player[] players)
		{
			string output = GetPlacesAfterGameLog(players);
			using (Task writeTask = File.AppendAllTextAsync(_pathToFile, output))
				await writeTask;
		}

		public async Task WritePlayersCardsAsync(Player[] players)
		{
			string output = GetPlayersCardsLog(players);
			using (Task writeTask = File.AppendAllTextAsync(_pathToFile, output))
				await writeTask;
		}

		public async Task WriteClientSendMessageAsync(Player player, string message)
		{
			string output = GetClientSendMessageLog(player, message);
			output = output + (LogCodesConsts.NEW_LINE);
			using (Task writeTask = File.AppendAllTextAsync(_pathToFile, output))
				await writeTask;
		}

		public async Task WriteStartRoundAsync(int roundNumber)
		{
			string output = GetRoundStartedLog(roundNumber);
			output = output + (LogCodesConsts.NEW_LINE);
			using (Task writeTask = File.AppendAllTextAsync(_pathToFile, output))
				await writeTask;
		}

		public async Task WriteStartTrickAsync(int trickNumber, int roundNumber)
		{
			string output = GetTrickStartedLog(trickNumber, roundNumber);
			output = output + (LogCodesConsts.NEW_LINE);
			using (Task writeTask = File.AppendAllTextAsync(_pathToFile, output))
				await writeTask;
		}

		#endregion

	}
}
