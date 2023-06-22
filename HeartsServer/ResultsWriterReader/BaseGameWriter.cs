using Hearts_server.GameLogic;
using Hearts_server.GameLogic.Cards;
using Hearts_server.ResultsWriter;

namespace Hearts_server.ResultsWriterReader
{
    public abstract class BaseGameWriter : IGameWriterHandler
    {
        #region Messages constants

        protected const string CODE_CONST = "_Code_";
        protected const string TIME_CONST = "_Time_";
        protected const string CODE_AND_TIME_CONST = CODE_CONST +  " " + TIME_CONST + ": ";
        protected const string PLAYER_NAME_CONST = "_Player_Name_";
        protected const string PLAYER_ID_CONST = "_Player_Id_";
        protected const string PLAYER_NAME_AND_ID_CONST = PLAYER_NAME_CONST + ", ID: " + PLAYER_ID_CONST;
        protected const string CARDS_CONST = "_Cards_";
        protected const string CARD_CONST = "_Card_";
        protected const string POINTS_CONST = "_Points";
        protected const string PLACE_CONST = "_Place_";
        protected const string MESSAGE_CONST = "_Message_";
        protected const string USER_CONNECTED_CONST = CODE_AND_TIME_CONST + " player " + PLAYER_NAME_AND_ID_CONST + " connected with server";
        protected const string USER_CLICKED_START_GAME_CONST = CODE_AND_TIME_CONST + " player " + PLAYER_NAME_AND_ID_CONST + " clicked start game";
        protected const string GAME_STARTED_CONST = CODE_AND_TIME_CONST + ": Game started";
        protected const string PLAYER_GOT_CARDS_CONST = CODE_AND_TIME_CONST + " player " + PLAYER_NAME_AND_ID_CONST + " got cards: " + CARDS_CONST;
        protected const string PLAYER_GAVE_CARDS_EXCHANGE_CONST = CODE_AND_TIME_CONST + " player " + PLAYER_NAME_AND_ID_CONST + " gave cards " + CARDS_CONST + " for exchange to " + PLAYER_NAME_AND_ID_CONST;
        protected const string PLAYER_RECEIVED_CARDS_EXCHANGE_CONST = CODE_AND_TIME_CONST + " player " + PLAYER_NAME_AND_ID_CONST + " received cards " + CARDS_CONST + " for exchange to " + PLAYER_NAME_AND_ID_CONST;
        protected const string PLAYER_THREW_CARD_CONST = CODE_AND_TIME_CONST + " player " + PLAYER_NAME_AND_ID_CONST + " threw card " + CARD_CONST;
        protected const string TRICK_CONST = CODE_AND_TIME_CONST + " player " + PLAYER_NAME_AND_ID_CONST + " won trick with cards: " + CARDS_CONST;
        protected const string PLAYER_POINTS_IN_ROUND_CONST = CODE_AND_TIME_CONST + " player's " + PLAYER_NAME_AND_ID_CONST + " points in that round: " + POINTS_CONST;
        protected const string PLAYER_POINTS_AFTER_ROUND_CONST = CODE_AND_TIME_CONST + " player's " + PLAYER_NAME_AND_ID_CONST + " points after that round: " + POINTS_CONST;
        protected const string PLAYER_PLACE_AFTER_GAME_CONST = CODE_AND_TIME_CONST + " player's " + PLAYER_NAME_AND_ID_CONST + " place in game: " + PLACE_CONST;
        protected const string PLAYER_CARDS_CONST = CODE_AND_TIME_CONST + " player's " + PLAYER_NAME_AND_ID_CONST + " cards: " + CARDS_CONST;
        protected const string CLIENT_SEND_MESSAGE = CODE_AND_TIME_CONST + " player " + PLAYER_NAME_AND_ID_CONST + " send message: " + MESSAGE_CONST;

        #endregion

        protected IGameWriterHandler _nextWriter;

        public IGameWriterHandler SetNext(IGameWriterHandler writer)
        {
            _nextWriter = writer;
            return _nextWriter;
        }

        public virtual void HandleWriteUserConnected(Player player) { }
        public virtual void HandleWriteUserClickedStartGame(Player player) { }
        public virtual void HandleWriteStartedGame() { }
        public virtual void HandleWritePlayersGotCards(Player[] players) { }
        public virtual void HandleWritePlayerGaveCardsExchange(Player player, Card[] cards) { }
        public virtual void HandleWritePlayerReceivedCardsExchange(Player player, Card[] cards) { }
        public virtual void HandleWritePlayerThrewCard(Player player, Card card) { }
        public virtual void HandleWriteTrick(Trick trick) { }
        public virtual void HandleWritePlayersPointsInRound(Player[] players) { }
        public virtual void HandleWritePlayersPointsAfterRound(Player[] players) { }
        public virtual void HandleWritePlacesAfterGame(Player[] players) { }
        public virtual void HandleWritePlayersCards(Player[] players) { }
        public virtual void HandleWriteClientSendMessage(string message) { }

    }
}
