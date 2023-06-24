using Hearts_server.GameLogic;
using Hearts_server.GameLogic.Cards;
using HeartsServer.GameLogic.Shuffle;
using HeartsServer.ResultsWriterReader;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeartsServer.GameLogic.Tests.Writers.Tests.ILogWriter.Tests
{
    [TestClass]
    public class ConsoleLogWriterTest
    {
        public static StringWriter stringWriter;
        public static TextWriter standardOut;
        public static ConsoleLogWriter writer;

        [ClassInitialize()]
        public static void ClassInitialize(TestContext testContext)
        {
            writer = new ConsoleLogWriter();
            standardOut = Console.Out;
            stringWriter = new StringWriter();
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
            Console.SetOut(standardOut);
        }

        [TestCleanup()]
        public void TestCleanup()
        {
            Player.ResetIdCounter();
            stringWriter.GetStringBuilder().Clear();
        }

        [TestInitialize()]
        public void TestInitialize()
        {
            Console.SetOut(stringWriter);
        }

        [TestMethod]
        public void WriteUserConnected_CorrectOutput()
        {
            Player player = new("John");
            writer.WriteUserConnected(player);

            Assert.AreEqual(String.Concat("C1 ", DateTime.Now.ToString("G"), ":  player ", player.Name, ", ID: ", player.Id, " connected with server\r\n"), stringWriter.ToString());
        }

        [TestMethod]
        public void WriteUserClickedStartGame_CorrectOutput()
        {
            Player player = new("John");
            writer.WriteUserClickedStartGame(player);

            Assert.AreEqual(String.Concat("C2 ", DateTime.Now.ToString("G"), ":  player ", player.Name, ", ID: ", player.Id, " clicked start game\r\n"), stringWriter.ToString());
        }

        [TestMethod]
        public void WriteStartGame_CorrectOutput()
        {
            writer.WriteStartedGame();

            Assert.AreEqual(String.Concat("C3 ", DateTime.Now.ToString("G"), ": Game started\r\n"), stringWriter.ToString());
        }

        [TestMethod]
        public void WritePlayersGotCards_CorrectOutput()
        {
            var players = new Player[] { new Player("John"), new Player("Paul"), new Player("Michael"), new Player("Joseph") };
            var cards = new StandardRandomShuffleEngine().Shuffle(Game.Instance.GetPackOfCards());

            for (int i = 0; i < Consts.PLAYERS_NUMBER; i++)
                players[i].SetCards(cards[i]);

            writer.WritePlayersGotCards(players);

            StringBuilder output = new StringBuilder();

            for (int i = 0; i < Consts.PLAYERS_NUMBER; i++)
            {
                output.Append(String.Concat("C4 ", DateTime.Now.ToString("G"), ":  player ", players[i].Name, ", ID: ", players[i].Id, " got cards: "));
                foreach (var card in players[i].OwnCards)
                    output.Append(card.ToString()).Append(", ");

                output.Append("\r\n");
            }

            Assert.AreEqual(output.ToString(), stringWriter.ToString());
        }

        [TestMethod]
        public void WritePlayerGaveCardsExchangeLog_CorrectOutput()
        {
            Player playerFrom = new Player("John");
            Player playerTo = new Player("Adam");
            Card[] cards = new Card[] { new Card(
                CardValue.Five, CardColour.Spade),
                new Card(CardValue.Ten, CardColour.Diamond),
                new Card(CardValue.Queen, CardColour.Club) };

            writer.WritePlayerGaveCardsExchange(playerFrom, playerTo, cards);

            StringBuilder cardsStr = new StringBuilder();
            foreach (var card in cards)
                cardsStr.Append(card.ToString() + ", ");

            string output = String.Concat("C5 ", DateTime.Now.ToString("G"), ":  player ", playerFrom.Name, ", ID: ", playerFrom.Id, " gave cards ", cardsStr, " for exchange to ", playerTo.Name, ", ID: ", playerTo.Id, "\r\n");
            Assert.AreEqual(output, stringWriter.ToString());
        }

        [TestMethod]
        public void WritePlayerReceivedCardsExchangeLog_CorrectOutput()
        {
            Player playerFrom = new Player("John");
            Player playerTo = new Player("Adam");
            Card[] cards = new Card[] {
                new Card(CardValue.Five, CardColour.Spade),
                new Card(CardValue.Ten, CardColour.Diamond),
                new Card(CardValue.Queen, CardColour.Club) };

            writer.WritePlayerReceivedCardsExchange(playerFrom, playerTo, cards);

            StringBuilder cardsStr = new StringBuilder();
            foreach (var card in cards)
                cardsStr.Append(card.ToString() + ", ");

            string output = String.Concat("C6 ", DateTime.Now.ToString("G"), ":  player ", playerFrom.Name, ", ID: ", playerFrom.Id, " received cards ", cardsStr, " for exchange from ", playerTo.Name, ", ID: ", playerTo.Id, "\r\n");
            Assert.AreEqual(output, stringWriter.ToString());
        }

        [TestMethod]
        public void WritePlayerThrewCardLog_CorrectOutput()
        {
            Player player = new Player("John");
            Card card = new Card(CardValue.Ace, CardColour.Heart);

            writer.WritePlayerThrewCard(player, card);

            Assert.AreEqual(String.Concat("C7 ", DateTime.Now.ToString("G"), ":  player ", player.Name, ", ID: ", player.Id.ToString(), " threw card ", card.ToString(), "\r\n"), stringWriter.ToString());
        }

        [TestMethod]
        public void WriteTrick_CorrectOutput()
        {
            Player playerWhoWonTrick = new("Michael"), playerWhoStarted = new("Adam");
            Card[] cards = new Card[] {
                    new Card(CardValue.Two, CardColour.Club),
                    new Card(CardValue.Eight, CardColour.Club),
                    new Card(CardValue.Three, CardColour.Heart),
                    new Card(CardValue.Jack, CardColour.Club)};

            Trick trick = new Trick(cards, playerWhoWonTrick, playerWhoStarted);

            writer.WriteTrick(trick);

            StringBuilder cardsStr = new();
            foreach (Card card in cards)
                cardsStr.Append(card.ToString() + ", ");

            string output = String.Concat("C8 ", DateTime.Now.ToString("G"), ":  player ", playerWhoWonTrick.Name, ", ID: ", playerWhoWonTrick.Id.ToString(), " won trick with cards: ", cardsStr.ToString(), "\r\n");

            Assert.AreEqual(output, stringWriter.ToString());
        }

        [TestMethod]
        public void WritePlayersPointsInRound_CorrectOutput()
        {
            Player[] players = new Player[] {
                new Player("John"),
                new Player("Adam"),
                new Player("Peter"),
                new Player("David")
            };



            //for(int i = 0; i < Consts.PLAYERS_NUMBER; i++)
            //{
            //    players[i]. = i;
            //}

            //writer.WritePlayersPointsInRound();
        }
    }
}
