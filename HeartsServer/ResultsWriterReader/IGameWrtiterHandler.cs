using Hearts_server.GameLogic;
using Hearts_server.GameLogic.Cards;

namespace Hearts_server.ResultsWriterReader
{
    public interface IGameWriterHandler
    {
        IGameWriterHandler SetNext(IGameWriterHandler handler);

        void HandleWriteTrick(Trick trick);
        void HandleWriteThrownCard(Card card);
    }
}
