using Hearts_server.GameLogic;
using Hearts_server.GameLogic.Cards;
using Hearts_server.ResultsWriter;
using Hearts_server.ResultsWriterReader;
using System.Diagnostics;
using System.Numerics;
using System.Text;

namespace HeartsServer.ResultsWriterReader
{
    //TODO: pomyslec czy nie przeniesc handlowych metod do BaseGameWriter
    //TODO: pomyslec czy nie sparametryzowac handlowych metod do jednej
    //TODO: Przeniesc do klasy bazowej replacowanie z metod Write...
    public class ConsoleLogWriter : BaseGameWriter, IFlatFileWriter
    {
        #region Handles
        public override void HandleWriteUserConnected(Player player)
        {
            WriteUserConnected(player);
            if (_nextWriter != null)
                _nextWriter.HandleWriteUserConnected(player);
        }
        public override void HandleWriteUserClickedStartGame(Player player)
        {
            WriteUserClickedStartGame(player);
            if (_nextWriter != null)
                _nextWriter.HandleWriteUserClickedStartGame(player);
        }
        public override void HandleWriteStartedGame()
        {
            WriteStartedGame();
            if (_nextWriter != null)
                _nextWriter.HandleWriteStartedGame();
        }
        public override void HandleWritePlayersGotCards(Player[] players)
        {
            WritePlayersGotCards(players);
            if (_nextWriter != null)
                _nextWriter.HandleWritePlayersGotCards(players);
        }
        public override void HandleWritePlayerGaveCardsExchange(Player player, Card[] cards)
        {
            WritePlayerGaveCardsExchange(player, cards);
            if (_nextWriter != null)
                _nextWriter.HandleWritePlayerGaveCardsExchange(player, cards);
        }
        public override void HandleWritePlayerReceivedCardsExchange(Player player, Card[] cards)
        {
            WritePlayerReceivedCardsExchange(player, cards);
            if (_nextWriter != null)
                _nextWriter.HandleWritePlayerReceivedCardsExchange(player, cards);
        }
        public override void HandleWriteTrick(Trick trick)
        {
            WriteTrick(trick);
            if (_nextWriter != null)
                _nextWriter.HandleWriteTrick(trick);
        }

        public override void HandleWritePlayerThrewCard(Player player, Card card)
        {
            WritePlayerThrewCard(player, card);
            if (_nextWriter != null)
                _nextWriter.HandleWritePlayerThrewCard(player, card);
        }
        public override void HandleWritePlayersPointsInRound(Player[] players)
        {
            WritePlayersPointsInRound(players);
            if (_nextWriter != null)
                _nextWriter.HandleWritePlayersPointsInRound(players);
        }
        public override void HandleWritePlayersPointsAfterRound(Player[] players)
        {
            WritePlayersPointsAfterRound(players);
            if (_nextWriter != null)
                _nextWriter.HandleWritePlayersPointsAfterRound(players);
        }
        public override void HandleWritePlacesAfterGame(Player[] players)
        {
            WritePlacesAfterGame(players);
            if (_nextWriter != null)
                _nextWriter.HandleWritePlacesAfterGame(players);
        }
        public override void HandleWritePlayersCards(Player[] players)
        {
            WritePlayersCards(players);
            if (_nextWriter != null)
                _nextWriter.HandleWritePlayersCards(players);
        }
        public override void HandleWriteClientSendMessage(string message)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IFlatFileWriter methods

        public void WriteUserConnected(Player player)
        {
            string output = USER_CONNECTED_CONST
                .Replace(CODE_CONST, "C1")
                .Replace(TIME_CONST, DateTime.Now.ToString())
                .Replace(PLAYER_NAME_CONST, player.Name)
                .Replace(PLAYER_ID_CONST, player.Id.ToString());

            Console.WriteLine(output);
        }
        public void WriteUserClickedStartGame(Player player)
        {
            string output = USER_CLICKED_START_GAME_CONST
                .Replace(CODE_CONST, "C2")
                .Replace(TIME_CONST, DateTime.Now.ToString())
                .Replace(PLAYER_NAME_CONST, player.Name)
                .Replace(PLAYER_ID_CONST, player.Id.ToString());

            Console.WriteLine(output);
        }
        public void WriteStartedGame()
        {
            string output = GAME_STARTED_CONST
                .Replace(CODE_CONST, "C3")
                .Replace(TIME_CONST, DateTime.Now.ToString());

            Console.WriteLine(output);
        }
        public void WritePlayersGotCards(Player[] players)
        {
            foreach (Player player in players)
            {
                StringBuilder cards = new StringBuilder();
                foreach (var card in player.OwnCards)
                    cards.Append(card.ToString() + " ");

                string output = PLAYER_GOT_CARDS_CONST
                    .Replace(CODE_CONST, "C4")
                    .Replace(TIME_CONST, DateTime.Now.ToString())
                    .Replace(PLAYER_NAME_CONST, player.Name)
                    .Replace(PLAYER_ID_CONST, player.Id.ToString())
                    .Replace(CARDS_CONST, cards.ToString());

                Console.WriteLine(output);
            }
        }
        public void WritePlayerGaveCardsExchange(Player player, Card[] cards)
        {
            StringBuilder cardsStr = new StringBuilder();
            foreach (Card card in cards)
                cardsStr.Append(card.ToString() + " ");

            string output = PLAYER_GAVE_CARDS_EXCHANGE_CONST
                .Replace(CODE_CONST, "C5")
                .Replace(TIME_CONST, DateTime.Now.ToString())
                .Replace(PLAYER_NAME_CONST, player.Name)
                .Replace(PLAYER_ID_CONST, player.Id.ToString())
                .Replace(CARDS_CONST, cardsStr.ToString());

            Console.WriteLine(output);
        }
        public void WritePlayerReceivedCardsExchange(Player player, Card[] cards)
        {

            StringBuilder cardsStr = new StringBuilder();
            foreach (Card card in cards)
                cardsStr.Append(card.ToString() + " ");

            string output = PLAYER_RECEIVED_CARDS_EXCHANGE_CONST
                .Replace(CODE_CONST, "C6")
                .Replace(TIME_CONST, DateTime.Now.ToString())
                .Replace(PLAYER_NAME_CONST, player.Name)
                .Replace(PLAYER_ID_CONST, player.Id.ToString())
                .Replace(CARDS_CONST, cardsStr.ToString());

            Console.WriteLine(output);
        }
        public void WritePlayerThrewCard(Player player, Card card)
        {
            string output = PLAYER_THREW_CARD_CONST
                .Replace(CODE_CONST, "C7")
                .Replace(TIME_CONST, DateTime.Now.ToString())
                .Replace(PLAYER_NAME_CONST, player.Name)
                .Replace(PLAYER_ID_CONST, player.Id.ToString())
                .Replace(CARD_CONST, card.ToString());

            Console.WriteLine(output);
        }
        public void WriteTrick(Trick trick)
        {
            StringBuilder cardsStr = new StringBuilder();
            foreach (Card card in trick.Cards)
                cardsStr.Append(card.ToString() + " ");


            string output = TRICK_CONST
                .Replace(CODE_CONST, "C8")
                .Replace(TIME_CONST, DateTime.Now.ToString())
                .Replace(PLAYER_NAME_CONST, trick.Owner.Name)
                .Replace(PLAYER_ID_CONST, trick.Owner.Id.ToString())
                .Replace(CARDS_CONST, cardsStr.ToString());

            Console.WriteLine(output);
        }
        public void WritePlayersPointsInRound(Player[] players)
        {
            foreach (Player player in players)
            {
                string output = PLAYER_POINTS_IN_ROUND_CONST
                    .Replace(CODE_CONST, "C9")
                    .Replace(TIME_CONST, DateTime.Now.ToString())
                    .Replace(PLAYER_NAME_CONST, player.Name)
                    .Replace(PLAYER_ID_CONST, player.Id.ToString())
                    .Replace(POINTS_CONST, player.PointsInRound.ToString());

                Console.WriteLine(output);
            }
        }
        public void WritePlayersPointsAfterRound(Player[] players)
        {
            foreach (Player player in players)
            {
                string output = PLAYER_POINTS_AFTER_ROUND_CONST
                    .Replace(CODE_CONST, "C10")
                    .Replace(TIME_CONST, DateTime.Now.ToString())
                    .Replace(PLAYER_NAME_CONST, player.Name)
                    .Replace(PLAYER_ID_CONST, player.Id.ToString())
                    .Replace(POINTS_CONST, player.Points.ToString());

                Console.WriteLine(output);
            }
        }
        public void WritePlacesAfterGame(Player[] players)
        {
            foreach (Player player in players)
            {
                string output = PLAYER_PLACE_AFTER_GAME_CONST
                    .Replace(CODE_CONST, "C11")
                    .Replace(TIME_CONST, DateTime.Now.ToString())
                    .Replace(PLAYER_NAME_CONST, player.Name)
                    .Replace(PLAYER_ID_CONST, player.Id.ToString())
                    .Replace(PLACE_CONST, player.Place.ToString());

                Console.WriteLine(output);
            }
        }
        public void WritePlayersCards(Player[] players)
        {
            foreach (Player player in players)
            {
                StringBuilder cardsStr = new StringBuilder();
                foreach (Card card in player.OwnCards)
                    cardsStr.Append(card.ToString() + " ");

                string output = PLAYER_CARDS_CONST
                    .Replace(CODE_CONST, "C12")
                    .Replace(TIME_CONST, DateTime.Now.ToString())
                    .Replace(PLAYER_NAME_CONST, player.Name)
                    .Replace(PLAYER_ID_CONST, player.Id.ToString())
                    .Replace(CARDS_CONST, cardsStr.ToString());

                Console.WriteLine(output);
            }
        }
        public void WriteClientSendMessage(Player player, string message)
        {
            string ouptut = CLIENT_SEND_MESSAGE
                .Replace(CODE_CONST, "C12")
                .Replace(TIME_CONST, DateTime.Now.ToString())
                .Replace(PLAYER_NAME_CONST, player.Name)
                .Replace(PLAYER_ID_CONST, player.Id.ToString())
                .Replace(MESSAGE_CONST, message);
        }

        #endregion
    }
}
