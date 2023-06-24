using Hearts_server.GameLogic;
using Hearts_server.GameLogic.Cards;

namespace Hearts_server.ResultsWriterReader
{
    public interface IGameWriterHandler
    {
        IGameWriterHandler SetNext(IGameWriterHandler handler);

        void HandleWriteUserConnected(Player player);
        void HandleWriteUserClickedStartGame(Player player);
        void HandleWriteStartedGame();
        void HandleWritePlayersGotCards(Player[] players);
        void HandleWritePlayerGaveCardsExchange(Player playerFrom, Player playerTo, Card[] cards);
        void HandleWritePlayerReceivedCardsExchange(Player playerFrom, Player playerTo, Card[] cards);
        void HandleWritePlayerThrewCard(Player player, Card card);
        void HandleWriteTrick(Trick trick);
        void HandleWritePlayersPointsInRound(Player[] players);
        void HandleWritePlayersPointsAfterRound(Player[] players, int roundNumber);
        void HandleWritePlacesAfterGame(Player[] players);
        void HandleWritePlayersCards(Player[] players);
        void HandleWriteClientSendMessage(string message);
    }
}
