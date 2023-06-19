using Hearts_server.GameLogic;
using Hearts_server.GameLogic.Cards;
using System;

namespace HeartsServer.GameLogic.History
{
	public class PlayerCardsHistory
	{
		public Player PlayerId { get; set; }
		public string PlayerName { get; set; }
		public List<Card> Cards { get; set; }
	}
}
