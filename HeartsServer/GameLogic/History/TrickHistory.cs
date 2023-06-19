using Hearts_server.GameLogic;
using Hearts_server.GameLogic.Cards;

namespace HeartsServer.GameLogic.History
{
	public class TrickHistory
	{
		public int TrickNumber { get; set; }
		public Player WhoWon { get; set; }
		public List<QueueHistory> Queue { get; set; }
		public List<PlayerPointsHistory> PointsAfterTrick { get; set; }
		public PlayerCardsHistory[] PlayerCardsAfterTrick { get; set; }
	}
}