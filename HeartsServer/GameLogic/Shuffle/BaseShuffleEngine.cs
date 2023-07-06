using Hearts_server.GameLogic.Cards;
using Hearts_server.GameLogic.Shuffle;
using HeartsServer.GameLogic.Consts;

namespace HeartsServer.GameLogic.Shuffle
{
    public abstract class BaseShuffleEngine : IShuffle
    {
        public virtual List<List<Card>> Shuffle(Card[] cards)
        {
            return new List<List<Card>>();
        }

        protected bool IsAnyCardNull(Card[] cards) => cards.Any(c => c is null);
        protected bool IsCorrectNumberOfCards(Card[] cards) => cards.Length == NumbersConsts.CARDS_NUMBER_CONST;
        protected bool AreAllCardsDifferent(Card[] cards) => cards.Distinct(new CardComparer()).Count() == cards.Length;
    }
}
