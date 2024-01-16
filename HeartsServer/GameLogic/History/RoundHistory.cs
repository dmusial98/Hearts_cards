namespace HeartsServer.GameLogic.History
{
	public class RoundHistory : BaseHistory
	{
		public int RoundNumber { get; set; }
		public List<PlayerCardsHistory> PlayerCardsBeforeExchange { get; set; } = new();
		public List<ExchangeHistory> Exchange { get; set; } = new();
		public List<PlayerCardsHistory> PlayerCardsAfterExchange { get; set; } = new();
        public List<TrickHistory> Tricks { get; set; } = new();
        public List<PlayerHistory> PlayerAfterRound { get; set; }
    }
}