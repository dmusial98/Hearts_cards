namespace HeartsServer.GameLogic.History
{
	public class PlayerPointsHistory
	{
        public bool IsWrittenInFiles { get; set; }
        public int PlayerId { get; set; }
		public string PlayerName { get; set; }
		public int Points { get; set; }
	}
}
