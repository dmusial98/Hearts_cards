namespace HeartsServer.GameLogic.History
{
	public class PlayerHistory
	{
        public bool IsWrittenInFiles { get; set; }
        public int Id { get; set; }
		public string Name { get; set; }
		public int Points { get; set; }
		public int Bonuses { get; set; }
	}
}