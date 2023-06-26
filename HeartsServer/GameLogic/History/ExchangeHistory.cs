using Hearts_server.GameLogic.Cards;

namespace HeartsServer.GameLogic.History
{
	public class ExchangeHistory
	{
		public bool IsWrittenInFiles { get; set; } 
		public int IdPlayer { get; set; }
		public int ToWho { get; set; }
		public int FromWho { get; set; }
		public List<Card> Gave { get; set; }
		public List<Card> Received { get; set; }
		public List<Card> CardsAfter { get; set; }
	}
}
