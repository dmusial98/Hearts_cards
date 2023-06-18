using Hearts_server.GameLogic.Cards;

namespace Hearts_server.GameLogic
{
    //nasza polska lewa

    public class Trick
    {
        public Card[] cards { get; set; } = new Card[4];
        public Player owner { get; set; }

        public Trick() { }
    }
}
