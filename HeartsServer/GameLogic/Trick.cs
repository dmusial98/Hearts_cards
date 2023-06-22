using Hearts_server.GameLogic.Cards;

namespace Hearts_server.GameLogic
{
    //nasza polska lewa
    public class Trick
    {
        public Card[] Cards { get; set; } = new Card[4];
        public Player Owner { get; set; }

        public Trick() { }
    }
}
