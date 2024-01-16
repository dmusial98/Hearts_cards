using System;

namespace HeartsServer.GameLogic.History
{
    public class GameHistory : BaseHistory
    {
        public bool IsTerminated { get; set; } //czy skonczona
        public DateTime StartTime { get; set; }
        public List<PlayerHistory> Players { get; set; } = new();
        public List<RoundHistory> Rounds { get; set; } = new();
        public List<Exception> Errors { get; set; } = new();
    }
}
