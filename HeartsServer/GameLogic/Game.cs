using Hearts_server.GameLogic.Cards;
using Hearts_server.GameLogic.Shuffle;
using Hearts_server.GameLogic.SignalR;
using Hearts_server.GameLogic.Timer;
using Hearts_server.ResultsWriterReader;
using HeartsServer.GameLogic;
using HeartsServer.GameLogic.History;

namespace Hearts_server.GameLogic
{
    //TODO: po rozpoczeciu gry ustawic graczom clickedStartGame na false
    //TODO: sprawdzic czy nie potrzebny Singleton bezpieczny watkowo
    public class Game
    {

        private static Game _instance;
        public static Game Instance
        {
            get
            {
                if (_instance is null)
                    _instance = new Game();
                return _instance;
            }
        }


        internal IShuffle shuffleLogic;
        internal IConnectable connection;
        internal ITImer gameTimer;
        internal BaseGameWriter gameWriter;
        internal IGameReader gameReader;
        internal GameHistory history = new GameHistory();

        internal Player[] players = new Player[Consts.PLAYERS_NUMBER_CONST];
        internal Card[] cards = new Card[Consts.CARDS_NUMBER_CONST];
        internal Trick actualTrick = new Trick();
        internal int roundIndex = 1;
        internal int playerIndex;

        private Game()
        {
            cards = GetPackOfCards();
        }

        public void SetFields(IShuffle shuffle)
        {
            if (shuffleLogic != null)
                this.shuffleLogic = shuffle;
        }

        public Card[] GetPackOfCards()
        {
            var cards = new Card[Consts.CARDS_NUMBER_CONST];

            for (int i = 0; i < Enum.GetNames(typeof(CardColour)).Length; i++)
                for (int j = 2; j < Enum.GetNames(typeof(CardValue)).Length + 2; j++)
                    cards[i * Enum.GetNames(typeof(CardValue)).Length + j - 2] = new Card(j, i);

            return cards;
        }

        public void AddPlayer(string playerName)
        {
            throw new NotImplementedException();
        }

        public void RemovePlayer(int id)
        {
            throw new NotImplementedException();
        }

        public void LoadSettings() { throw new NotImplementedException(); }
        public void StartGame()
        {
            throw new NotImplementedException();


        }
        public void DoExchange() { throw new NotImplementedException(); }
        public void NewRound() { throw new NotImplementedException(); }
        public void NextPlayer()
        {
            throw new NotImplementedException();

            if (playerIndex != 3)
                playerIndex++;
            else
                playerIndex = 0;
        }
        public void AddTrickToPlayer(Player player)
        {
            throw new NotImplementedException();
        }

    }
}
