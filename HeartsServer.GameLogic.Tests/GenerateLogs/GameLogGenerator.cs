using Hearts_server.GameLogic.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HeartsServer.GameLogic.Tests.GenerateLogs
{
	[TestClass]
	public class GameLogGenerator
	{
		[TestMethod]
		public void GenerateLogsForOneGame()
		{
			//zaladuj karty z pliku
			//przydziel graczom 

			var cards = LoadCardsFromFile(0);

		}

		public List<List<Card>> LoadCardsFromFile(int roundId)
		{
			var text = File.ReadAllText($@"cards_shuffle\shuffle{roundId}.json");
			var result = JsonSerializer.Deserialize<List<List<Card>>>(text);

			return result;
		}


		public void GenerateLogsForOneRound(int roundID) 
		{
		
		}

		public void GenerateLogsForOneTrick() { }

	}
}
