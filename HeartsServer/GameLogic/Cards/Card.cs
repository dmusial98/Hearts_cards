using System.Data.Common;

namespace Hearts_server.GameLogic.Cards
{
    public class Card : IComparable<Card>
    {
        public CardValue Value { get; private set; }
        public CardColour Colour { get; private set; }
        public Card(int value, int colour)
        {
            Value = (CardValue)value;
            Colour = (CardColour)colour;
        }

        public int CompareTo(Card? other)
        {
            if (other == null)
                return -1;

            if (this.Colour > other.Colour)
                return 1;
            else if (this.Colour < other.Colour)
                return -1;
            else
            {
                if (this.Value > other.Value)
                    return 1;
                else if (this.Value < other.Value)
                    return -1;
                else
                    return 0;
            }
        }
    }
}
