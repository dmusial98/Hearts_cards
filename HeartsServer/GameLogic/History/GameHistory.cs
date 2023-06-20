namespace HeartsServer.GameLogic.History
{
    public class GameHistory
    {
        public List<RoundHistory> Rounds { get; set; } = new List<RoundHistory>();
        public List<PlayerHistory> Players { get; set; }
    }
}
