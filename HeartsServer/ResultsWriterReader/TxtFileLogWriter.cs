using System.Globalization;
using Hearts_server.GameLogic.Cards;
using Hearts_server.GameLogic;
using Hearts_server.ResultsWriter;
using Hearts_server.ResultsWriterReader;
using System.Numerics;
using HeartsServer.GameLogic.Consts;
using HeartsServer.GameLogic.History;

namespace HeartsServer.ResultsWriterReader
{
    public class TxtFileLogWriter : BaseTextGameWriter, ILogWriter
    {
        private string _pathToFile;
        public TxtFileLogWriter() => setPathToFile(DateTime.Now);
        public TxtFileLogWriter(DateTime dateTime) => setPathToFile(dateTime);

        //public async Task WriteStartedGameAsync()
        //public async Task WriteStartedGameAsync(DateTime datetime)
        //public async Task WriteStartRoundAsync(int roundNumber)
        //public async Task WritePlayersGotCardsAsync(Player[] players)
        //public async Task WritePlayerGaveCardsExchangeAsync(Player playerFrom, Player playerTo, Card[] cards)
        //public async Task WritePlayerReceivedCardsExchangeAsync(Player playerFrom, Player playerTo, Card[] cards)
        //public async Task WriteStartTrickAsync(int trickNumber, int roundNumber)
        //public async Task WritePlayerThrewCardAsync(Player player, Card card)
        //public async Task WriteTrickAsync(Trick trick)
        //public async Task WritePlayersPointsInRoundAsync(Player[] players, int roundNumber)
        //public async Task WritePlayersPointsAfterRoundAsync(Player[] players, int roundNumber)
        //public async Task WritePlacesAfterGameAsync(Player[] players)
        //public async Task WritePlayersCardsAsync(Player[] players)

        void setPathToFile(DateTime dateTime)
        {
            _pathToFile = $"LogFiles/{dateTime.ToString(new CultureInfo("pl-PL"))}_logs.txt".Replace(" ", "_").Replace(":", "_");
        }

        public async Task SaveTxtFileLogFileFromGameHistory(GameHistory gameHistory)
        {
            if (gameHistory == null || gameHistory.StartTime == DateTime.MinValue)
                return;

            setPathToFile(gameHistory.StartTime);

            await WriteStartedGameAsync(gameHistory.StartTime);
            foreach (var round in gameHistory.Rounds)
            {
                await WriteStartRoundAsync(round.RoundNumber);
                for (int i = 0; i < NumbersConsts.PLAYERS_NUMBER_CONST; i++)
                {
                    round.PlayerCardsBeforeExchange[i].PlayerName = gameHistory.Players.First(p => p.PlayerId == round.PlayerCardsBeforeExchange[i].PlayerId).Name;
                    round.PlayerCardsAfterExchange[i].PlayerName = gameHistory.Players.First(p => p.PlayerId == round.PlayerCardsAfterExchange[i].PlayerId).Name;
                }
                await WritePlayersGotCardsAsync(round.PlayerCardsBeforeExchange.ToArray());
                foreach (var exchange in round.Exchange)
                {
                    await WritePlayerGaveCardsExchangeAsync(new PlayerHistory
                    {
                        PlayerId = exchange.IdPlayer,
                        Name = gameHistory.Players.First(p => p.PlayerId == exchange.IdPlayer).Name
                    },
                    new PlayerHistory
                    {
                        PlayerId = exchange.ToWho,
                        Name = gameHistory.Players.First(p => p.PlayerId == exchange.ToWho).Name
                    },
                    exchange.Gave.ToArray());
                    await WritePlayerReceivedCardsExchangeAsync(new PlayerHistory
                    {
                        PlayerId = exchange.FromWho,
                        Name = gameHistory.Players.First(p => p.PlayerId == exchange.FromWho).Name
                    },
                    new PlayerHistory
                    {
                        PlayerId = exchange.IdPlayer,
                        Name = gameHistory.Players.First(p => p.PlayerId == exchange.IdPlayer).Name
                    },
                    exchange.Received.ToArray()
                    );
                }
                await WritePlayersCardsAsync(round.PlayerCardsAfterExchange.ToArray());

                foreach (var trick in round.Tricks)
                {
                    await WriteStartTrickAsync(trick.TrickNumber, round.RoundNumber);
                    await WritePlayersCardsAsync(trick.PlayerCardsBeforeTrick.ToArray());
                    foreach (var queue in trick.Queue)
                    {
                        queue.PlayerName = gameHistory.Players.First(p => p.PlayerId == queue.PlayerId).Name;
                        await WritePlayerThrewCardAsync(queue);
                    }
                    trick.WhoWonName = gameHistory.Players.First(p => p.PlayerId == trick.WhoWonId).Name;
                    trick.WhoStartedName = gameHistory.Players.First(p => p.PlayerId == trick.WhoStartedId).Name;
                    await WriteTrickAsync(trick);

                    foreach (var player in trick.PointsAfterTrick)
                        player.Name = gameHistory.Players.First(p => p.PlayerId == player.PlayerId).Name;

                    await WritePlayersPointsInRoundAsync(trick.PointsAfterTrick.ToArray(), round.RoundNumber);
                } //trick

                await WritePlayersPointsAfterRoundAsync(round.PlayersAfterRound.ToArray(), round.RoundNumber);
                await WritePlacesAfterGameAsync(round.PlayersAfterRound.ToArray());

            } //round
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

        public async Task WriteStartedGameAsync(DateTime datetime)
        {
            string output = GetStartedGameLog(datetime);
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

        public async Task WritePlayersGotCardsAsync(PlayerCardsHistory[] playersCards)
        {
            string output = GetPlayersGotCardsLog(playersCards);

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

        public async Task WritePlayerGaveCardsExchangeAsync(PlayerHistory playerFrom, PlayerHistory playerTo, Card[] cards)
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

        public async Task WritePlayerReceivedCardsExchangeAsync(PlayerHistory playerFrom, PlayerHistory playerTo, Card[] cards)
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

        public async Task WritePlayerThrewCardAsync(QueueHistory queueHistory)
        {
            string output = GetPlayerThrewCardLog(queueHistory);
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
        public async Task WriteTrickAsync(TrickHistory trick)
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

        public async Task WritePlayersPointsInRoundAsync(PlayerHistory[] players, int roundNumber)
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

        public async Task WritePlayersPointsAfterRoundAsync(PlayerHistory[] players, int roundNumber)
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

        public async Task WritePlacesAfterGameAsync(PlayerHistory[] players)
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

        public async Task WritePlayersCardsAsync(PlayerCardsHistory[] cardsHistory)
        {
            string output = GetPlayersCardsLog(cardsHistory);
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
