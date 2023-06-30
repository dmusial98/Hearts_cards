namespace HeartsServer.GameLogic.History
{
	public class RoundHistory
	{
		public bool IsWrittenInFiles { get; set; }
		public int RoundNumber { get; set; }
		public List<PlayerPointsHistory> PointsAfterRound { get; set; }
		public List<TrickHistory> Tricks { get; set; } = new List<TrickHistory>();
		public PlayerCardsHistory[] PlayerCardsBefore { get; set; }
		public List<ExchangeHistory> Exchange { get; set; }
	}
}