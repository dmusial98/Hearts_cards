namespace HeartsServer.GameLogic.History
{
    public class GameHistory
    {
        public bool IsWrittenInFiles { get; set; }
        public List<RoundHistory> Rounds { get; set; } = new();
        public List<PlayerHistory> Players { get; set; } = new();
    }
}
