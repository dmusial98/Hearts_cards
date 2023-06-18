using Hearts_server.GameLogic.Cards;
using Hearts_server.GameLogic.Shuffle;
using HeartsServer.GameLogic.Shuffle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeartsServer.GameLogic.Tests
{
    public abstract class IShuffleTestBase
    {
        public void Shuffle_CorrectList(IShuffle shuffleEng)
        {
            IShuffle shuffleEngine = shuffleEng;
            var result = shuffleEngine.Shuffle(GetCards());

            Assert.IsNotNull(result);
            Assert.AreEqual(Consts.PLAYERS_NUMBER, result.Count);
            Assert.AreEqual(Consts.CARDS_FOR_PLAYER, result[0].Count);
            Assert.AreEqual(Consts.CARDS_FOR_PLAYER, result[1].Count);
            Assert.AreEqual(Consts.CARDS_FOR_PLAYER, result[2].Count);
            Assert.AreEqual(Consts.CARDS_FOR_PLAYER, result[3].Count);
        }

        public void Shuffle_ToLittleCards(IShuffle shuffleEng)
        {
            try
            {
                IShuffle shuffleEngine = new GiveInOneColour();
                var result = shuffleEngine.Shuffle(new Card[4] { new Card(2, 2), new Card(3, 2), new Card(4, 2), new Card(4, 2) });
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
                IShuffle shuffleEngine = new GiveInOneColour();
                var cards = GetCards();
                cards[10] = new Card(2, 3);
                var result = shuffleEngine.Shuffle(cards);
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
                IShuffle shuffleEngine = new GiveInOneColour();
                var result = shuffleEngine.Shuffle(null);
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
                IShuffle shuffleEngine = new GiveInOneColour();
                var cards = GetCards();
                cards[10] = null;
                var result = shuffleEngine.Shuffle(cards);
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            

            Assert.Fail();
        }

        public Card[] GetCards()
        {
            Card[] cards = new Card[Consts.CARDS_NUMBER];

            for (int i = 0; i < Enum.GetValues(typeof(CardColour)).Length; i++)
                for (int j = 2; j < Enum.GetValues(typeof(CardValue)).Length + 2; j++)
                    cards[i * Enum.GetValues(typeof(CardValue)).Length + j - 2] = new Card(j, i);

            return cards;
        }
    }
}
