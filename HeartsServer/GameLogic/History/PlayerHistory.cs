namespace HeartsServer.GameLogic.History
{
	public class PlayerHistory : BaseHistory
	{
		public int PlayerId { get; set; }
		public string Name { get; set; }
		public int? PointsInGame { get; set; }
		public int? PointsInRound { get; set; }
		public int? Bonuses { get; set; }
		public int? Place { get; set; }
	}
}