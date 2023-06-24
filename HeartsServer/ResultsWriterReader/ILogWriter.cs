using Hearts_server.GameLogic;
using Hearts_server.GameLogic.Cards;

namespace Hearts_server.ResultsWriter
{
    public interface ILogWriter
    {
        public void WriteUserConnected(Player player);
        public void WriteUserClickedStartGame(Player player);
        public void WriteStartedGame();
        public void WritePlayersGotCards(Player[] players);
        public void WritePlayerGaveCardsExchange(Player playerFrom, Player playerTo, Card[] cards);
        public void WritePlayerReceivedCardsExchange(Player playerFrom, Player playerTo, Card[] cards);
        public void WritePlayerThrewCard(Player player, Card card);
        public void WriteTrick(Trick trick);
        public void WritePlayersPointsInRound(Player[] players);
        public void WritePlayersPointsAfterRound(Player[] players, int roundNumber);
        public void WritePlacesAfterGame(Player[] players);
        public void WritePlayersCards(Player[] players);
        public void WriteClientSendMessage(Player player, string message);


    }
}
