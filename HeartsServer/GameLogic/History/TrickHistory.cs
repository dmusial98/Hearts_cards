using Hearts_server.GameLogic;
using Hearts_server.GameLogic.Cards;

namespace HeartsServer.GameLogic.History
{
	public class TrickHistory
	{
		public bool IsWrittenInFiles { get; set; }
		public int TrickNumber { get; set; }
		public int WhoWon { get; set; }
		public int WhoStarted { get; set; }
		public List<QueueHistory> Queue { get; set; } = new();
		public List<PlayerPointsHistory> PointsAfterTrick { get; set; } = new();
		public List<PlayerCardsHistory> PlayerCardsAfterTrick { get; set; } = new();
	}
}