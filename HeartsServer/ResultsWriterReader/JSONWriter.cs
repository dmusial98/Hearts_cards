using Hearts_server.GameLogic;
using Hearts_server.GameLogic.Cards;
using Hearts_server.ResultsWriter;

namespace Hearts_server.ResultsWriterReader
{
    public class JsonWriter : BaseGameWriter, IGameWriter
    {
        public override void HandleWriteTrick(Trick trick)
        {
            WriteTrick(trick);
            if(base._nextWriter is not null) 
                base._nextWriter.HandleWriteTrick(trick);
        }

        public override void HandleWriteThrownCard(Card card)
        {
            WriteThrownCard(card);
            if(base._nextWriter is not null)
                base._nextWriter.HandleWriteThrownCard(card);
        }

        public void WriteTrick(Trick trick)
        {
            throw new NotImplementedException();
        }

        public void WriteThrownCard(Card card)
        {
            throw new NotImplementedException();
        }
    }
}
