using Hearts_server.GameLogic.Cards;
using Hearts_server.GameLogic;
using Hearts_server.ResultsWriter;
using Hearts_server.ResultsWriterReader;

namespace HeartsServer.ResultsWriterReader
{
    public class TxtFileLogWriter : BaseGameWriter, ILogWriter
    {
        private string _pathToFile;
        public TxtFileLogWriter()
        {
            _pathToFile = $"LogFiles/{DateTime.Now.ToString("G")}_logs.txt".Replace(" ", "_").Replace(":", "_");
        }






        #region ILogWriter methods

        public void WriteUserConnected(Player player)
        {

        }

        public async Task WriteUserConnectedAsync(Player player)
        {
            string output = GetUserConnectedLog(player);
            output = output + ("\r\n");
            using (Task writeTask = File.AppendAllTextAsync(_pathToFile, output))
                await writeTask;


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
            Console.Write(GetPlayersGotCardsLog(players));
        }

        public void WritePlayerGaveCardsExchange(Player playerFrom, Player playerTo, Card[] cards)
        {
            Console.WriteLine(GetPlayerGaveCardsExchangeLog(playerFrom, playerTo, cards));
        }

        public void WritePlayerReceivedCardsExchange(Player playerFrom, Player playerTo, Card[] cards)
        {
            Console.WriteLine(GetPlayerReceivedCardsExchangeLog(playerFrom, playerTo, cards));
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
            Console.Write(GetPlayersPointsInRoundLog(players));
        }

        public void WritePlayersPointsAfterRound(Player[] players, int roundNumber)
        {
            Console.Write(GetPlayersPointsAfterRoundLog(players, roundNumber));
        }

        public void WritePlacesAfterGame(Player[] players)
        {
            Console.Write(GetPlacesAfterGameLog(players));
        }

        public void WritePlayersCards(Player[] players)
        {
            Console.Write(GetPlayersCardsLog(players));
        }

        public void WriteClientSendMessage(Player player, string message)
        {
            Console.WriteLine(GetClientSendMessageLog(player, message));
        }

        #endregion

    }
}
