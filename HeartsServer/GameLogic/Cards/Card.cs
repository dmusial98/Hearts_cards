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

    public class CardComparer : IEqualityComparer<Card>
    {
        public bool Equals(Card? x, Card? y)
        {
            if(Object.ReferenceEquals(x, y)) 
                return true;

            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            return x.Value == y.Value && x.Colour == y.Colour;
        }

        public int GetHashCode(Card card)
        {
            //Check whether the object is null
            if (Object.ReferenceEquals(card, null)) return 0;

            //Get hash code for the Name field if it is not null.
            int hashValue = card.Value.GetHashCode();

            //Get hash code for the Code field.
            int hashColour = card.Colour.GetHashCode();

            //Calculate the hash code for the product.
            return hashValue ^ hashColour;
        }
    }
}
