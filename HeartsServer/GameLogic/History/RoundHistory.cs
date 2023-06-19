namespace HeartsServer.GameLogic.History
{
	public class RoundHistory
	{
		public int RoundNumber;
		public List<TrickHistory> TricksHistory { get; set; } = new List<TrickHistory>();
		public PlayerCardsHistory[] PlayerCardsHistoryBeforeStart { get; set; }
		public PlayerCardsHistory[] PlayerCardsHistoryAfterExchange { get; set; }
	}
}