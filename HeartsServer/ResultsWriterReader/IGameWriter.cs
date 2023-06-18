using Hearts_server.GameLogic;
using Hearts_server.GameLogic.Cards;

namespace Hearts_server.ResultsWriter
{
    public interface IGameWriter
    {
        public void WriteTrick(Trick trick);
        public void WriteThrownCard(Card card);
    }
}
