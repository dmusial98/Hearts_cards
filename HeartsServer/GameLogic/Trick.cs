using Hearts_server.GameLogic.Cards;

namespace Hearts_server.GameLogic
{
    //nasza polska lewa
    public class Trick
    {
        public Card[] Cards { get; set; }
        public Player Owner { get; set; }
        public Player WhoStarted { get; set; }
        public Trick() { }
        public Trick(Card[] cards, Player owner, Player whoStarted)
        {
            Cards = cards;
            Owner = owner;
            WhoStarted = whoStarted;
        }
    }
}
