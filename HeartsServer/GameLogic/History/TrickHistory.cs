using Hearts_server.GameLogic;
using Hearts_server.GameLogic.Cards;

namespace HeartsServer.GameLogic.History
{
	public class TrickHistory : BaseHistory
	{
		public int TrickNumber { get; set; }
		public int WhoWonId { get; set; }
		public string WhoWonName {get; set;}
		public int WhoStartedId { get; set; }
		public string WhoStartedName {get; set;}
		public List<QueueHistory> Queue { get; set; } = new();
		public List<PlayerHistory> PointsAfterTrick { get; set; } = new();
		public List<PlayerCardsHistory> PlayerCardsBeforeTrick { get; set; } = new();
	}
}