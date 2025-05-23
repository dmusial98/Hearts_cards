﻿using Newtonsoft.Json;
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

		[JsonConstructor]
		public Card(CardValue value, CardColour colour)
		{
			Value = value;
			Colour = colour;
		}

		public bool IsScoring() => (Colour == CardColour.Heart || (Value == CardValue.Queen && Colour == CardColour.Spade ));

		//TODO: te logike z porownywaniem wszedzie wrzucic do jakiejs osobnej klasy, zeby nie kopiowac kodu

		public int CompareTo(Card other)
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

		public override string ToString()
		{
			return $"{Value} {Colour}";
		}
	}

	public class CardComparer : IEqualityComparer<Card>, IComparer<Card>
	{
		public bool Equals(Card x, Card y)
		{
			if (Object.ReferenceEquals(x, y))
				return true;

			if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
				return false;

			return x.Value == y.Value && x.Colour == y.Colour;
		}

		public int GetHashCode(Card card)
		{
			if (Object.ReferenceEquals(card, null))
				return 0;

			int hashValue = card.Value.GetHashCode();
			int hashColour = card.Colour.GetHashCode();

			return hashValue ^ hashColour;
		}

		public int Compare(Card x, Card y)
		{
			if (x == null)
			{
				if (y == null)
					return 0;
				else
					return -1;
			}
			else
			{
				if (y == null)
					return 1;
				if (x.Colour > y.Colour)
					return 1;
				else if (x.Colour < y.Colour)
					return -1;
				else
				{
					if (x.Value > y.Value)
						return 1;
					else if (x.Value < y.Value)
						return -1;
					else
						return 0;
				}
			}
		}
	}
}
