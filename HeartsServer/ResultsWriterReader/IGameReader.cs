using HeartsServer.GameLogic.History;
using Microsoft.AspNetCore.SignalR;

namespace Hearts_server.ResultsWriterReader
{
    public interface IGameReader
    {
        public Task<GameHistory> GetGameHistoryAsync();
    }
}
