using Hearts_server.ResultsWriterReader;
using HeartsServer.GameLogic.History;

namespace HeartsServer.ResultsWriterReader
{
    public class ExcelFileReader : GameFileReader, IGameReader
    {
        public override async Task<GameHistory> GetGameHistoryAsync()
        {
            GameHistory gameHistory = new GameHistory();
            var file = GetFile();

            //zainstalowac EPPLUS do Excela

            return gameHistory;
        }
    }
}
