using Hearts_server.GameLogic.Cards;
using HeartsServer.GameLogic;
using Microsoft.AspNetCore.WebUtilities;

namespace Hearts_server.GameLogic
{
    //nasza polska lewa
    public class Trick
    {
        public Card[] Cards { get; set; }
        public Player Owner { get; set; }
        public Player WhoStarted { get; set; }
        public int Points
        {
            get
            {
                int points = Cards.Count(c => c.Colour == CardColour.Heart);
                if (Cards.Contains(new Card(CardValue.Queen, CardColour.Spade), new CardComparer()))
                    points += Consts.QUEEN_SPADE_POINTS_CONST;

                return points;
            }
        }
        public Trick() { }
        public Trick(Card[] cards, Player owner, Player whoStarted)
        {
            Cards = cards;
            Owner = owner;
            WhoStarted = whoStarted;
        }
    }
}
