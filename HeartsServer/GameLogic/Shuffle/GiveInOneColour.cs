using Hearts_server.GameLogic.Cards;
using Hearts_server.GameLogic.Shuffle;

namespace HeartsServer.GameLogic.Shuffle
{
    public class GiveInOneColour : IShuffle
    {
        public List<List<Card>> Shuffle(Card[] cards) 
        {
            List<List<Card>> cardsForPlayers = new List<List<Card>>();

            for(int i = 0; i < Enum.GetNames(typeof(CardColour)).Length; i++)
            {
                var inOneColour = cards.Where(c => (int)c.Colour == i).ToList();
                cardsForPlayers[i] = inOneColour;
            }

            return cardsForPlayers;
        }
    }
}
