namespace HeartsServer.GameLogic.History
{
	public class RoundHistory
	{
		public bool IsWrittenInFiles { get; set; }
		public int RoundNumber { get; set; }
		public List<PlayerPointsHistory> PointsAfterRound { get; set; }
		public List<TrickHistory> Tricks { get; set; } = new();
		public List<PlayerCardsHistory> PlayerCardsBefore { get; set; } = new();
		public List<ExchangeHistory> Exchange { get; set; } = new();
	}
}