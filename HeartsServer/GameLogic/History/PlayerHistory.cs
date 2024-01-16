namespace HeartsServer.GameLogic.History
{
	public class PlayerHistory : BaseHistory
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int Points { get; set; }
		public int Bonuses { get; set; }
		public int Place { get; set; }
	}
}