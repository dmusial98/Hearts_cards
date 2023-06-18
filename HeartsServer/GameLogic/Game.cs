using Hearts_server.GameLogic.Cards;
using Hearts_server.GameLogic.Shuffle;
using Hearts_server.GameLogic.SignalR;
using Hearts_server.GameLogic.Timer;
using Hearts_server.History;
using Hearts_server.ResultsWriterReader;

namespace Hearts_server.GameLogic
{
    //TODO: po rozpoczeciu gry ustawic graczom clickedStartGame na false
    //TODO: sprawdzic czy nie potrzebny Singleton bezpieczny watkowo
    public class Game
    {
        public const int CARDS_NUMBER = 52;
        public const int PLAYERS_NUMBER = 4;

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

        internal Player[] players = new Player[PLAYERS_NUMBER];
        internal Card[] cards = new Card[CARDS_NUMBER];
        internal Trick actualTrick = new Trick();
        internal int roundIndex = 1;
        internal int playerIndex;

        private Game()
        {
            for (int i = 0; i < Enum.GetNames(typeof(CardColour)).Length; i++)
                for (int j = 2; j < Enum.GetNames(typeof(CardValue)).Length + 2; j++)
                    cards[i * Enum.GetNames(typeof(CardValue)).Length + j - 2] = new Card(j, i);
        }

        public void SetFields(IShuffle shuffle)
        {
            this.shuffleLogic = shuffle;
        }

        public void AddPlayer(string playerName)
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
