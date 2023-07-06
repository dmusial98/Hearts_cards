namespace HeartsServer.GameLogic.History
{
	public class GameHistory
	{
		public DateTime StartTime { get; set; }
		public bool IsWrittenInFiles { get; set; }
		public List<RoundHistory> Rounds { get; set; } = new();
		public List<PlayerHistory> Players { get; set; } = new();
	}
}
