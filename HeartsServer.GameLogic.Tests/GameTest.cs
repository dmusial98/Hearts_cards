using Hearts_server.GameLogic;
using HeartsServer.GameLogic.Shuffle;

namespace HeartsServer.GameLogic.Tests
{
    [TestClass]
    public class GameTest
    {
        readonly Game game = Game.Instance;

        [TestMethod]
        public void Game_GetInstance_CreatedNewInstance()
        {
            Assert.IsNotNull(game);
        }

        [TestMethod]
        public void Game_GetInstance_InstancesAreTheSame()
        {
            Game game1 = Game.Instance;

            Assert.AreEqual(game1, game);
        }

        [TestMethod]
        public void Game_AddPlayers_PlayersAreAdded()
        {
            game.AddPlayer("Player's Name");
            game.AddPlayer("Name too");

            Assert.AreEqual(2, game.players.Count());
        }

        [TestMethod]
        public void Game_AddPlayers_NameIsCorrect()
        {
            string name = "My name 123";
            game.AddPlayer(name);

            Assert.AreEqual(name, game.players[0].Name);
        }

        [TestMethod]
        public void Game_AddPlayers_IdsAreCorrect()
        {
            Add4Players();

            Assert.AreEqual(0, game.players[0].Id);
            Assert.AreEqual(1, game.players[1].Id);
            Assert.AreEqual(2, game.players[2].Id);
            Assert.AreEqual(3, game.players[3].Id);
        }

        [TestMethod]
        public void Game_AddShuffleLogic_IShuffleIsNotNull()
        {
            Add4Players();

            game.SetFields(new GiveInOneColourShuffleEngine());

            Assert.IsNotNull(game.shuffleLogic);
        }

        [TestMethod]
        public void Game_AddShuffleLogic_DoPlayersHaveCardsInSameColour()
        {
            Add4Players();
            game.SetFields(new GiveInOneColourShuffleEngine());
            //TODO: dokonczyc

        }

        public void Add4Players()
        {
            Game game = Game.Instance;
            game.AddPlayer("name1");
            game.AddPlayer("name2");
            game.AddPlayer("name3");
            game.AddPlayer("name4");
        }
    }
}