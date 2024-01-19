using Hearts_server.GameLogic.Cards;

namespace HeartsServer.GameLogic.History
{
	public class QueueHistory : BaseHistory
	{
		public int PlayerId { get; set; }
		public string PlayerName {get; set;}
		public Card Card { get; set; }
	}
}
