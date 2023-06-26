using Hearts_server.GameLogic;
using Hearts_server.GameLogic.Cards;

namespace Hearts_server.ResultsWriterReader
{
	public interface IGameWriterHandler
	{
		IGameWriterHandler SetNext(IGameWriterHandler handler);

		Task HandleWriteUserConnectedAsync(Player player);
		Task HandleWriteUserClickedStartGameAsync(Player player);
		Task HandleWriteStartedGameAsync();
		Task HandleWritePlayersGotCardsAsync(Player[] players);
		Task HandleWritePlayerGaveCardsExchangeAsync(Player playerFrom, Player playerTo, Card[] cards);
		Task HandleWritePlayerReceivedCardsExchangeAsync(Player playerFrom, Player playerTo, Card[] cards);
		Task HandleWritePlayerThrewCardAsync(Player player, Card card);
		Task HandleWriteTrickAsync(Trick trick);
		Task HandleWritePlayersPointsInRoundAsync(Player[] players);
		Task HandleWritePlayersPointsAfterRoundAsync(Player[] players, int roundNumber);
		Task HandleWritePlacesAfterGameAsync(Player[] players);
		Task HandleWritePlayersCardsAsync(Player[] players);
		Task HandleWriteClientSendMessageAsync(Player player, string message);
	}
}
