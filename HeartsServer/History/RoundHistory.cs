namespace Hearts_server.History
{
    public class RoundHistory
    {
        public List<TrickHistory> TricksHistory { get; set; } = new List<TrickHistory>();
        public PlayerCardsHistory[] PlayerCardsHistoryBeforeStart { get; set; }
        public PlayerCardsHistory[] PlayerCardsHistoryAfterExchange { get; set; }
    }
}