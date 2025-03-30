using Hearts_server.GameLogic;
using Hearts_server.GameLogic.Cards;
using Newtonsoft.Json;

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

            for (int fileNumber = 0; fileNumber < 21; fileNumber++)
            {
                var cards = LoadCardsFromFile(fileNumber);

                Player[] players = { new Player("Adam"), new Player("Dawid"), new Player("Rafal"), new Player("Pawel") };

                for (int i = 0; i < cards.Count; i++)
                {
                    players[i].SetCards(cards[i]);
                    cards[i] = players[i].OwnCards.ToList();
                }

                SaveToFile(fileNumber, cards);
            }
        }

        public List<List<Card>> LoadCardsFromFile(int roundId)
        {
            var text = File.ReadAllText($@"cards_shuffle/shuffle{roundId}.json");
            var result = JsonConvert.DeserializeObject<List<List<Card>>>(text);

            return result;
        }

        public void SaveToFile(int roundId, List<List<Card>> cards)
        {
            File.Delete($@"cards_shuffle\shuffle{roundId}.json");

            File.WriteAllText($@"cards_shuffle\shuffle{roundId}.json", JsonConvert.SerializeObject(cards, new Newtonsoft.Json.Converters.StringEnumConverter()));
        }


        public void GenerateLogsForOneRound(int roundID)
        {

        }

        public void GenerateLogsForOneTrick() { }

    }
}
