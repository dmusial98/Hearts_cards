using HeartsServer.GameLogic.Shuffle;

namespace HeartsServer.GameLogic.Tests.ShuffleEngine.Tests
{
	[TestClass]
	public class StandardRandomShuffleEngineTest : IShuffleTestBase
	{
		public TestContext? testContext { get; set; }

		[TestMethod]
		public void StandardRandomShuffleEngine_Shuffle_CorrectList()
		{
			Shuffle_CorrectList(new StandardRandomShuffleEngine());
			//WriteCards(testContext);
			foreach (var playerCards in Cards)
			{
				foreach (var card in playerCards)
					testContext?.WriteLine(card.ToString());

				testContext?.Write("\n\n");

			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void StandardRandomShuffleEngine_Shuffle_TooLittleCards()
		{
			Shuffle_ToLittleCards(new StandardRandomShuffleEngine());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void StandardRandomShuffleEngine_Shuffle_CardsAreNotDifferent()
		{
			Shuffle_CardsAreNotDifferent(new StandardRandomShuffleEngine());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void StandardRandomShuffleEngine_Shuffle_CardsAreNull()
		{
			Shuffle_CardsAreNull(new StandardRandomShuffleEngine());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void StandardRandomShuffleEngine_Shuffle_OneCardIsNull()
		{
			Shuffle_OneCardIsNull(new StandardRandomShuffleEngine());
		}
	}
}
