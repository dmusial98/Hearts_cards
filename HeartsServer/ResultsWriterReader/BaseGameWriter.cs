using Hearts_server.GameLogic;
using Hearts_server.GameLogic.Cards;
using Hearts_server.ResultsWriter;

namespace Hearts_server.ResultsWriterReader
{
    public abstract class BaseGameWriter : IGameWriterHandler
    {
        protected IGameWriterHandler _nextWriter;

        public IGameWriterHandler SetNext(IGameWriterHandler writer)
        {
            _nextWriter = writer;
            return _nextWriter;
        }

        public virtual void HandleWriteTrick(Trick trick) { }
        public virtual void HandleWriteThrownCard(Card card) { }
        
    }
}
