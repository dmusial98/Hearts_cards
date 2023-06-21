using Hearts_server.GameLogic.Cards;
using Hearts_server.GameLogic.Shuffle;

namespace HeartsServer.GameLogic.Shuffle
{
    public class GiveInOneColourShuffleEngine : BaseShuffleEngine, IShuffle
    {
        public override List<List<Card>> Shuffle(Card[] cards) 
        {
            if(cards is null || base.IsAnyCardNull(cards))
                throw new ArgumentNullException(nameof(cards));
            if(!base.IsCorrectNumberOfCards(cards) || !AreAllCardsDifferent(cards))
                throw new ArgumentException(nameof(cards));
            

            List<List<Card>> cardsForPlayers = new List<List<Card>>();

            for(int i = 0; i < Enum.GetNames(typeof(CardColour)).Length; i++)
            {
                var inOneColour = cards.Where(c => (int)c.Colour == i).ToList();
                cardsForPlayers.Add(inOneColour);
            }

            return cardsForPlayers;
        }
    }
}
