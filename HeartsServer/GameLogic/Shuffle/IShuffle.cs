using Hearts_server.GameLogic.Cards;

namespace Hearts_server.GameLogic.Shuffle
{
    public interface IShuffle
    {
        List<List<Card>> Shuffle(Card[] cards);
    }
}
