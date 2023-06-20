using Hearts_server.GameLogic.Cards;
using Hearts_server.GameLogic.Shuffle;
using HeartsServer.GameLogic.Shuffle;


namespace HeartsServer.GameLogic.Tests.ShuffleEngine.Tests
{
    [TestClass]
    public class GiveInOneColourTest : IShuffleTestBase
    {
        [TestMethod]
        public void GiveInOneColour_Shuffle_CorrectList()
        {
            Shuffle_CorrectList(new GiveInOneColour());
        }

        [TestMethod]
        public void Shuffle_CardsWithSameColour()
        {
            IShuffle shuffleEngine = new GiveInOneColour();
            var result = shuffleEngine.Shuffle(GetCards());

            //cards in the same colour
            Assert.AreEqual(1, result[0].DistinctBy(c => c.Colour).Count());
            Assert.AreEqual(1, result[1].DistinctBy(c => c.Colour).Count());
            Assert.AreEqual(1, result[2].DistinctBy(c => c.Colour).Count());
            Assert.AreEqual(1, result[3].DistinctBy(c => c.Colour).Count());

            //different values
            Assert.AreEqual(Consts.CARDS_FOR_PLAYER, result[0].DistinctBy(c => c.Value).Count());
            Assert.AreEqual(Consts.CARDS_FOR_PLAYER, result[1].DistinctBy(c => c.Value).Count());
            Assert.AreEqual(Consts.CARDS_FOR_PLAYER, result[2].DistinctBy(c => c.Value).Count());
            Assert.AreEqual(Consts.CARDS_FOR_PLAYER, result[3].DistinctBy(c => c.Value).Count());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GiveInOneColour_Shuffle_TooLittleCards()
        {
            Shuffle_ToLittleCards(new GiveInOneColour());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GiveInOneColour_Shuffle_CardsAreNotDifferent()
        {
            Shuffle_CardsAreNotDifferent(new GiveInOneColour());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GiveInOneColour_Shuffle_CardsAreNull()
        {
            Shuffle_CardsAreNull(new GiveInOneColour());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GiveInOneColour_Shuffle_OneCardIsNull()
        {
            Shuffle_OneCardIsNull(new GiveInOneColour());
        }

    }
}
