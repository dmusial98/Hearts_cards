using Hearts_server.GameLogic.Cards;
using Hearts_server.GameLogic.Shuffle;
using HeartsServer.GameLogic.Shuffle;

namespace HeartsServer.GameLogic.Tests.ShuffleEngine.Tests
{
	[TestClass]
	public abstract class ShuffleTestBase
	{
		protected List<List<Card>> Cards { get; set; }

		public void Shuffle_CorrectList(IShuffle shuffleEng)
		{
			IShuffle shuffleEngine = shuffleEng;
			var result = shuffleEngine.Shuffle(GetCards());

			Cards = result;

			Assert.IsNotNull(result);
			Assert.AreEqual(Consts.PLAYERS_NUMBER_CONST, result.Count);

			for (int i = 0; i < Consts.PLAYERS_NUMBER_CONST; i++)
				Assert.AreEqual(Consts.CARDS_FOR_PLAYER_CONST, result[i].Count);
		}

		public void Shuffle_CardsAreDifferentInOutput(IShuffle shuffleEng)
		{
			IShuffle shuffleEngine = shuffleEng;
			var result = shuffleEngine.Shuffle(GetCards());

			Card[] array = new Card[Consts.CARDS_NUMBER_CONST];

			for (int i = 0; i < Consts.PLAYERS_NUMBER_CONST; i++)
				result[i].CopyTo(array, i * Consts.CARDS_FOR_PLAYER_CONST);

			var groupped = array.GroupBy(x => x).Where(x => x.Count() > 1);

			Assert.IsNotNull(groupped);
			Assert.AreEqual(0, groupped.Count());
		}

		public void Shuffle_ToLittleCards(IShuffle shuffleEng)
		{
			try
			{
				IShuffle shuffleEngine = new GiveInOneColourShuffleEngine();
				shuffleEngine.Shuffle(new Card[4] { new Card(2, 2), new Card(3, 2), new Card(4, 2), new Card(4, 2) });
			}
			catch (ArgumentException ex)
			{
				throw ex;
			}

			Assert.Fail();
		}


		public void Shuffle_CardsAreNotDifferent(IShuffle shuffleEng)
		{
			try
			{
				IShuffle shuffleEngine = new GiveInOneColourShuffleEngine();
				var cards = GetCards();
				cards[10] = new Card(2, 3);
				shuffleEngine.Shuffle(cards);
			}
			catch (ArgumentException ex)
			{
				throw ex;
			}

			Assert.Fail();
		}


		public void Shuffle_CardsAreNull(IShuffle shuffleEng)
		{
			try
			{
				IShuffle shuffleEngine = new GiveInOneColourShuffleEngine();
				shuffleEngine.Shuffle(null);
			}
			catch (ArgumentNullException ex)
			{
				throw ex;
			}

			Assert.Fail();
		}


		public void Shuffle_OneCardIsNull(IShuffle shuffleEng)
		{
			try
			{
				IShuffle shuffleEngine = new GiveInOneColourShuffleEngine();
				var cards = GetCards();
				cards[10] = null;
				shuffleEngine.Shuffle(cards);
			}
			catch (ArgumentNullException ex)
			{
				throw ex;
			}


			Assert.Fail();
		}

		public Card[] GetCards()
		{
			Card[] cards = new Card[Consts.CARDS_NUMBER_CONST];

			for (int i = 0; i < Enum.GetValues(typeof(CardColour)).Length; i++)
				for (int j = 2; j < Enum.GetValues(typeof(CardValue)).Length + 2; j++)
					cards[i * Enum.GetValues(typeof(CardValue)).Length + j - 2] = new Card(j, i);

			return cards;
		}

		public void WriteCards(TestContext? testContext)
		{
			foreach (var playerCards in Cards)
			{
				foreach (var card in playerCards)
					testContext?.WriteLine(card.ToString());

				testContext?.Write("\n\n");
			}

		}
	}


}
