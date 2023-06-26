using Hearts_server.GameLogic;
using Hearts_server.GameLogic.Cards;

namespace Hearts_server.ResultsWriter
{
    public interface ILogWriter
    {
        public Task WriteUserConnectedAsync(Player player);
        public Task WriteUserClickedStartGameAsync(Player player);
        public Task WriteStartedGameAsync();
        public Task WritePlayersGotCardsAsync(Player[] players);
        public Task WritePlayerGaveCardsExchangeAsync(Player playerFrom, Player playerTo, Card[] cards);
        public Task WritePlayerReceivedCardsExchangeAsync(Player playerFrom, Player playerTo, Card[] cards);
        public Task WritePlayerThrewCardAsync(Player player, Card card);
        public Task WriteTrickAsync(Trick trick);
        public Task WritePlayersPointsInRoundAsync(Player[] players);
        public Task WritePlayersPointsAfterRoundAsync(Player[] players, int roundNumber);
        public Task WritePlacesAfterGameAsync(Player[] players);
        public Task WritePlayersCardsAsync(Player[] players);
        public Task WriteClientSendMessageAsync(Player player, string message);


    }
}
