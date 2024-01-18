using Hearts_server.GameLogic.Cards;
using HeartsServer.GameLogic;
using HeartsServer.GameLogic.Consts;
using HeartsServer.GameLogic.Exceptions;
using System.Runtime.Serialization;

namespace Hearts_server.GameLogic
{
	public class Player
	{
		private static int _idCounter = 0;
		public static void ResetIdCounter() => _idCounter = 0;

		private int _points = 0;
		private int _pointsInRound = 0;
		readonly private List<Trick> tricks = new();
		private List<Card> cards;
		public int Place { get; set; }
		public int BonusesNumber { get; set; }
		private CardComparer cardComparer = new CardComparer();

		public Card[] OwnCards
		{
			get => cards?.ToArray();
		}
		public Trick[] Tricks
		{
			get => tricks?.ToArray();
		}

		public int Points
		{
			get { return _points; }
		}

		public int PointsInRound
		{
			get { return _pointsInRound; }
		}

		public int Id { get; private set; }
		public string Name { get; private set; }
		public bool ClickedStartGame { get; set; } = false;

		private static void incrementIdCounter() => _idCounter++;

		public Player(string name)
		{
			Name = name;
			Id = _idCounter;
			incrementIdCounter();
		}

        public Player(string name, int id)
        {
            Name = name;
            Id = id;
        }

        //add trick during round
        public void AddTrick(Trick trick)
		{
			tricks.Add(trick);
			_pointsInRound = CountPointsInTricks();
		}

		//set cards before new round
		public void SetCards(List<Card> cards)
		{
			if (cards == null)
				throw new ArgumentNullException();

			this.cards = cards;
			SortOwnCards();
		}

		public void SortOwnCards() => cards.Sort(cardComparer);

		public bool HasCard(Card card) => GetOwnCard(card) != null;

		private Card GetOwnCard(Card card) => cards.Where(c => c.Colour == card.Colour && card.Value == card.Value).FirstOrDefault();

		public Card ThrowCard(Card card)
		{
			var ownCard = GetOwnCard(card);
			
			if (ownCard == null)
				throw new NotExistedCardThrewException();

			cards.Remove(ownCard);

			return ownCard;
		}

		

		//add possible points at round from tricks cards
		public void CountPointsAfterRound() => _points += CountPointsInTricks();

		public void ClearTricks()
		{
			tricks.Clear();
		}

		public bool DoesHaveCardWithColour(CardColour colour) => OwnCards.Any(c => c.Colour == colour);

		private int CountPointsInTricks()
		{
			int points = tricks.Sum(t => t.Points);
			if (points == NumbersConsts.FULL_POINTS_IN_ROUND_CONST)
				points = -NumbersConsts.FULL_POINTS_IN_ROUND_CONST;

			return points;
		}
	}
}
