using Hearts_server.GameLogic;
using Hearts_server.GameLogic.Cards;
using Hearts_server.ResultsWriter;
using System.Text;
using System.Text.RegularExpressions;

namespace Hearts_server.ResultsWriterReader
{
    public abstract class BaseGameWriter : IGameWriterHandler
    {
        #region Messages constants

        protected const string CODE_CONST = "_Code_";
        protected const string TIME_CONST = "_Time_";
        protected const string CODE_AND_TIME_CONST = CODE_CONST + " " + TIME_CONST + ": ";
        protected const string PLAYER_NAME_CONST = "_Player_Name_";
        protected const string PLAYER_ID_CONST = "_Player_Id_";
        protected const string PLAYER_NAME_AND_ID_CONST = PLAYER_NAME_CONST + ", ID: " + PLAYER_ID_CONST;
        protected const string CARDS_CONST = "_Cards_";
        protected const string CARD_CONST = "_Card_";
        protected const string POINTS_CONST = "_Points";
        protected const string PLACE_CONST = "_Place_";
        protected const string MESSAGE_CONST = "_Message_";
        protected const string ROUND_NUMBER_CONST = "_Round_Number_";
        protected const string USER_CONNECTED_CONST = CODE_AND_TIME_CONST + " player " + PLAYER_NAME_AND_ID_CONST + " connected with server";
        protected const string USER_CLICKED_START_GAME_CONST = CODE_AND_TIME_CONST + " player " + PLAYER_NAME_AND_ID_CONST + " clicked start game";
        protected const string GAME_STARTED_CONST = CODE_AND_TIME_CONST + "Game started";
        protected const string PLAYER_GOT_CARDS_CONST = CODE_AND_TIME_CONST + " player " + PLAYER_NAME_AND_ID_CONST + " got cards: " + CARDS_CONST;
        protected const string PLAYER_GAVE_CARDS_EXCHANGE_CONST = CODE_AND_TIME_CONST + " player " + PLAYER_NAME_AND_ID_CONST + " gave cards " + CARDS_CONST + " for exchange to " + PLAYER_NAME_AND_ID_CONST;
        protected const string PLAYER_RECEIVED_CARDS_EXCHANGE_CONST = CODE_AND_TIME_CONST + " player " + PLAYER_NAME_AND_ID_CONST + " received cards " + CARDS_CONST + " for exchange from " + PLAYER_NAME_AND_ID_CONST;
        protected const string PLAYER_THREW_CARD_CONST = CODE_AND_TIME_CONST + " player " + PLAYER_NAME_AND_ID_CONST + " threw card " + CARD_CONST;
        protected const string TRICK_CONST = CODE_AND_TIME_CONST + " player " + PLAYER_NAME_AND_ID_CONST + " won trick with cards: " + CARDS_CONST;
        protected const string PLAYER_POINTS_IN_ROUND_CONST = CODE_AND_TIME_CONST + " player's " + PLAYER_NAME_AND_ID_CONST + " points in that round (" + ROUND_NUMBER_CONST + "): " + POINTS_CONST;
        protected const string PLAYER_POINTS_AFTER_ROUND_CONST = CODE_AND_TIME_CONST + " player's " + PLAYER_NAME_AND_ID_CONST + " points after that round (" + ROUND_NUMBER_CONST + "): " + POINTS_CONST;
        protected const string PLAYER_PLACE_AFTER_GAME_CONST = CODE_AND_TIME_CONST + " player's " + PLAYER_NAME_AND_ID_CONST + " place in game: " + PLACE_CONST;
        protected const string PLAYER_CARDS_CONST = CODE_AND_TIME_CONST + " player's " + PLAYER_NAME_AND_ID_CONST + " cards: " + CARDS_CONST;
        protected const string CLIENT_SEND_MESSAGE = CODE_AND_TIME_CONST + " player " + PLAYER_NAME_AND_ID_CONST + " send message: " + MESSAGE_CONST;

        #endregion

        readonly Regex regexPlayerName = new Regex(Regex.Escape(PLAYER_NAME_CONST));
        readonly Regex regexPlayerId = new Regex(Regex.Escape(PLAYER_ID_CONST));

        protected IGameWriterHandler _nextWriter;

        public IGameWriterHandler SetNext(IGameWriterHandler writer)
        {
            _nextWriter = writer;
            return _nextWriter;
        }

        #region Handle methods

        public virtual Task HandleWriteUserConnectedAsync(Player player) { return new Task(() => { }); }
        public virtual Task HandleWriteUserClickedStartGameAsync(Player player) { return new Task(() => { }); }
        public virtual Task HandleWriteStartedGameAsync() { return new Task(() => { }); }
        public virtual Task HandleWritePlayersGotCardsAsync(Player[] players) { return new Task(() => { }); }
        public virtual Task HandleWritePlayerGaveCardsExchangeAsync(Player playerFrom, Player playerTo, Card[] cards) { return new Task(() => { }); }
        public virtual Task HandleWritePlayerReceivedCardsExchangeAsync(Player playerFrom, Player playerTo, Card[] cards) { return new Task(() => { }); }
        public virtual Task HandleWritePlayerThrewCardAsync(Player player, Card card) { return new Task(() => { }); }
        public virtual Task HandleWriteTrickAsync(Trick trick) { return new Task(() => { }); }
        public virtual Task HandleWritePlayersPointsInRoundAsync(Player[] players, int roundNumber) { return new Task(() => { }); }
        public virtual Task HandleWritePlayersPointsAfterRoundAsync(Player[] players, int roundNumber) { return new Task(() => { }); }
        public virtual Task HandleWritePlacesAfterGameAsync(Player[] players) { return new Task(() => { }); }
        public virtual Task HandleWritePlayersCardsAsync(Player[] players) { return new Task(() => { }); }
        public virtual Task HandleWriteClientSendMessageAsync(Player player, string message) { return new Task(() => { }); }

        #endregion

        #region Get Logs Methods

        public string GetUserConnectedLog(Player player)
        {
            string output = USER_CONNECTED_CONST
                    .Replace(CODE_CONST, "C1")
                    .Replace(TIME_CONST, DateTime.Now.ToString("G"))
                    .Replace(PLAYER_NAME_CONST, player.Name)
                    .Replace(PLAYER_ID_CONST, player.Id.ToString());

            return output;
        }
        public string GetUserClickedStartGameLog(Player player)
        {
            string output = USER_CLICKED_START_GAME_CONST
                    .Replace(CODE_CONST, "C2")
                    .Replace(TIME_CONST, DateTime.Now.ToString("G"))
                    .Replace(PLAYER_NAME_CONST, player.Name)
                    .Replace(PLAYER_ID_CONST, player.Id.ToString());

            return output;
        }
        public string GetStartedGameLog()
        {
            string output = GAME_STARTED_CONST
                    .Replace(CODE_CONST, "C3")
                    .Replace(TIME_CONST, DateTime.Now.ToString("G"));

            return output;
        }
        public string GetPlayersGotCardsLog(Player[] players)
        {
            StringBuilder output = new StringBuilder();

            foreach (Player player in players)
            {
                StringBuilder cards = new StringBuilder();
                foreach (var card in player.OwnCards)
                    cards.Append(card.ToString() + ", ");

                output.Append(PLAYER_GOT_CARDS_CONST
                       .Replace(CODE_CONST, "C4")
                       .Replace(TIME_CONST, DateTime.Now.ToString("G"))
                       .Replace(PLAYER_NAME_CONST, player.Name)
                       .Replace(PLAYER_ID_CONST, player.Id.ToString())
                       .Replace(CARDS_CONST, cards.ToString())).Append("\r\n");
            }

            return output.ToString();
        }
        public string GetPlayerGaveCardsExchangeLog(Player playerFrom, Player playerTo, Card[] cards)
        {
            StringBuilder cardsStr = new StringBuilder();
            foreach (Card card in cards)
                cardsStr.Append(card.ToString() + ", ");

            string output = PLAYER_GAVE_CARDS_EXCHANGE_CONST
                    .Replace(CODE_CONST, "C5")
                    .Replace(TIME_CONST, DateTime.Now.ToString("G"))
                    .Replace(CARDS_CONST, cardsStr.ToString());

            output = regexPlayerName.Replace(output, playerFrom.Name, 1);
            output = regexPlayerName.Replace(output, playerTo.Name, 1);
            output = regexPlayerId.Replace(output, playerFrom.Id.ToString(), 1);
            output = regexPlayerId.Replace(output, playerTo.Id.ToString(), 1);

            return output;
        }
        public string GetPlayerReceivedCardsExchangeLog(Player playerFrom, Player playerTo, Card[] cards)
        {

            StringBuilder cardsStr = new StringBuilder();
            foreach (Card card in cards)
                cardsStr.Append(card.ToString() + ", ");

            string output = PLAYER_RECEIVED_CARDS_EXCHANGE_CONST
                    .Replace(CODE_CONST, "C6")
                    .Replace(TIME_CONST, DateTime.Now.ToString("G"))
                    .Replace(CARDS_CONST, cardsStr.ToString());

            output = regexPlayerName.Replace(output, playerFrom.Name, 1);
            output = regexPlayerName.Replace(output, playerTo.Name, 1);
            output = regexPlayerId.Replace(output, playerFrom.Id.ToString(), 1);
            output = regexPlayerId.Replace(output, playerTo.Id.ToString(), 1);

            return output;
        }
        public string GetPlayerThrewCardLog(Player player, Card card)
        {
            string output = PLAYER_THREW_CARD_CONST
                    .Replace(CODE_CONST, "C7")
                    .Replace(TIME_CONST, DateTime.Now.ToString("G"))
                    .Replace(PLAYER_NAME_CONST, player.Name)
                    .Replace(PLAYER_ID_CONST, player.Id.ToString())
                    .Replace(CARD_CONST, card.ToString());

            return output;
        }
        public string GetTrickLog(Trick trick)
        {
            StringBuilder cardsStr = new StringBuilder();
            foreach (Card card in trick.Cards)
                cardsStr.Append(card.ToString() + ", ");


            string output = TRICK_CONST
                    .Replace(CODE_CONST, "C8")
                    .Replace(TIME_CONST, DateTime.Now.ToString("G"))
                    .Replace(PLAYER_NAME_CONST, trick.Owner.Name)
                    .Replace(PLAYER_ID_CONST, trick.Owner.Id.ToString())
                    .Replace(CARDS_CONST, cardsStr.ToString());

            return output;
        }
        public string GetPlayersPointsInRoundLog(Player[] players, int roundNumber)
        {
            StringBuilder output = new();

            foreach (Player player in players)
            {
                output.Append(PLAYER_POINTS_IN_ROUND_CONST
                       .Replace(CODE_CONST, "C9")
                       .Replace(TIME_CONST, DateTime.Now.ToString("G"))
                       .Replace(PLAYER_NAME_CONST, player.Name)
                       .Replace(PLAYER_ID_CONST, player.Id.ToString())
                       .Replace(ROUND_NUMBER_CONST, roundNumber.ToString())
                       .Replace(POINTS_CONST, player.PointsInRound.ToString()))
                   .Append("\r\n");
            }

            return output.ToString();
        }
        public string GetPlayersPointsAfterRoundLog(Player[] players, int roundNumber)
        {
            StringBuilder output = new();

            foreach (Player player in players)
            {
                output.Append(PLAYER_POINTS_AFTER_ROUND_CONST
                        .Replace(CODE_CONST, "C10")
                        .Replace(TIME_CONST, DateTime.Now.ToString("G"))
                        .Replace(PLAYER_NAME_CONST, player.Name)
                        .Replace(PLAYER_ID_CONST, player.Id.ToString())
                        .Replace(ROUND_NUMBER_CONST, roundNumber.ToString())
                        .Replace(POINTS_CONST, player.Points.ToString()))
                    .Append("\r\n");
            }

            return output.ToString();
        }
        public string GetPlacesAfterGameLog(Player[] players)
        {
            StringBuilder output = new();
            foreach (Player player in players)
            {
                output.Append(PLAYER_PLACE_AFTER_GAME_CONST
                        .Replace(CODE_CONST, "C11")
                        .Replace(TIME_CONST, DateTime.Now.ToString("G"))
                        .Replace(PLAYER_NAME_CONST, player.Name)
                        .Replace(PLAYER_ID_CONST, player.Id.ToString())
                        .Replace(PLACE_CONST, player.Place.ToString()))
                    .Append("\r\n");
            }

            return output.ToString();
        }
        public string GetPlayersCardsLog(Player[] players)
        {
            StringBuilder output = new();
            foreach (Player player in players)
            {
                StringBuilder cardsStr = new StringBuilder();
                foreach (Card card in player.OwnCards)
                    cardsStr.Append(card.ToString() + ", ");

                output.Append(PLAYER_CARDS_CONST
                        .Replace(CODE_CONST, "C12")
                        .Replace(TIME_CONST, DateTime.Now.ToString("G"))
                        .Replace(PLAYER_NAME_CONST, player.Name)
                        .Replace(PLAYER_ID_CONST, player.Id.ToString())
                        .Replace(CARDS_CONST, cardsStr.ToString()))
                    .Append("\r\n");
            }

            return output.ToString();
        }
        public string GetClientSendMessageLog(Player player, string message)
        {
            string ouptut = CLIENT_SEND_MESSAGE
                    .Replace(CODE_CONST, "C13")
                    .Replace(TIME_CONST, DateTime.Now.ToString("G"))
                    .Replace(PLAYER_NAME_CONST, player.Name)
                    .Replace(PLAYER_ID_CONST, player.Id.ToString())
                    .Replace(MESSAGE_CONST, message);

            return ouptut;
        }
        #endregion

    }
}
