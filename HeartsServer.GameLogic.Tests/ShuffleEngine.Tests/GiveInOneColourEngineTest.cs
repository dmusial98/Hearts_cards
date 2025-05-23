﻿using Hearts_server.GameLogic.Cards;
using Hearts_server.GameLogic.Shuffle;
using HeartsServer.GameLogic.Consts;
using HeartsServer.GameLogic.Shuffle;


namespace HeartsServer.GameLogic.Tests.ShuffleEngine.Tests
{
	[TestClass]
	public class GiveInOneColourEngineTest : ShuffleTestBase
	{
		[TestMethod]
		public void GiveInOneColourEngine_Shuffle_CorrectList()
		{
			Shuffle_CorrectList(new GiveInOneColourShuffleEngine());
		}

		[TestMethod]
		public void GiveInOneColourEngine_Shuffle_CardsAreDifferentInOutput()
		{
			Shuffle_CardsAreDifferentInOutput(new GiveInOneColourShuffleEngine());
		}

		[TestMethod]
		public void GiveInOneColourEngine_Shuffle_CardsWithSameColour()
		{
			IShuffle shuffleEngine = new GiveInOneColourShuffleEngine();
			var result = shuffleEngine.Shuffle(GetCards());

			for (int i = 0; i < NumbersConsts.PLAYERS_NUMBER_CONST; i++)
			{
				//cards in the same colour
				Assert.AreEqual(1, result[i].DistinctBy(c => c.Colour).Count());
				//different values
				Assert.AreEqual(NumbersConsts.CARDS_FOR_PLAYER_CONST, result[i].DistinctBy(c => c.Value).Count());
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void GiveInOneColourEngine_Shuffle_TooLittleCards()
		{
			Shuffle_ToLittleCards(new GiveInOneColourShuffleEngine());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void GiveInOneColourEngine_Shuffle_CardsAreNotDifferent()
		{
			Shuffle_CardsAreNotDifferent(new GiveInOneColourShuffleEngine());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GiveInOneColourEngine_Shuffle_CardsAreNull()
		{
			Shuffle_CardsAreNull(new GiveInOneColourShuffleEngine());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GiveInOneColourEngine_Shuffle_OneCardIsNull()
		{
			Shuffle_OneCardIsNull(new GiveInOneColourShuffleEngine());
		}
	}
}
