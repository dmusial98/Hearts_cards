using Hearts_server.GameLogic.Cards;

namespace Hearts_server.GameLogic
{
    public class Player
    {
        private static int _idCounter = 0;
        public static void ResetIdCounter() => _idCounter = 0;

        private int _points = 0;
        private int _pointsInRound = 0;
        private List<Trick> tricks = new List<Trick>();
        private List<Card> cards;
        public int Place { get; set; }

        public Card[] OwnCards
        {
            get => cards.ToArray();
        }
        public Trick[] Tricks
        {
            get => tricks.ToArray();
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

        //add trick during round
        public void AddTrick(Trick trick)
        {
            throw new NotImplementedException();
        }

        //set cards before new round
        public void SetCards(List<Card> cards)
        {
            if (cards == null) 
                throw new ArgumentNullException();
            
            this.cards = cards;
        }

        //add possible points at round from tricks cards
        public void CountPointsAfterRound()
        {
            throw new NotImplementedException();
        }

        public bool DoesPlayerHaveCardWithColour(CardColour colour) => OwnCards.Any(c => c.Colour == colour);
    }
}
