using Hearts_server.GameLogic;
using Hearts_server.GameLogic.Cards;

namespace Hearts_server.History
{
    public class TrickHistory
    {
        public Card[] cards = new Card[4];
        public Player WhoWon { get; set; }
        public Player WhoStarted { get; set; }
        public PlayerCardsHistory[] CardsHistoryAfterTrick { get; set; }

    }
}