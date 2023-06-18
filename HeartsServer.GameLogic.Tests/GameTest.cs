using Hearts_server.GameLogic;

namespace HeartsServer.GameLogic.Tests
{
    [TestClass]
    public class GameTest
    {
        [TestMethod]
        public void Game_GetInstance_CreatedNewInstance()
        {
            Game game = Game.Instance;
            Assert.IsNotNull(game);
        }

        [TestMethod]
        public void Game_GetInstance_InstancesAreTheSame()
        {
            Game game1 = Game.Instance;
            Game game2 = Game.Instance;

            Assert.AreEqual(game1, game2);
        }

        [TestMethod]
        public void Game_AddPlayers_PlayersAreAdded()
        {
            Game game = Game.Instance;
            game.AddPlayer("Player's Name");
            game.AddPlayer("Name too");

            Assert.AreEqual(2, game.players.Count());
        }
        
    }
}