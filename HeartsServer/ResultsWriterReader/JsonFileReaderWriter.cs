using Hearts_server.GameLogic.Cards;
using HeartsServer.GameLogic.History;
using Newtonsoft.Json;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HeartsServer.ResultsWriterReader
{
    public static class JsonFileReaderWriter
    {
        public static async Task WriteGameHistory(GameHistory gameHistory)
        {
            if (gameHistory != null && gameHistory.StartTime != DateTime.MinValue)
            {
                var str = JsonConvert.SerializeObject(gameHistory, new Newtonsoft.Json.Converters.StringEnumConverter());
                await File.WriteAllTextAsync($@"LogFiles{GameLogic.Consts.LogCodesConsts.SLASH_DIRECTORY}{gameHistory.StartTime.ToString(new CultureInfo("pl-PL"))}_history.json".Replace(" ", "_").Replace(":", "_"), str);
            }
        }

        public static async Task<GameHistory> ReadGameHistory(string fileName)
        {
            var fileInfo = new FileInfo(fileName);
            if (fileInfo.Exists)
            {
                try
                {
                    var result = await File.ReadAllTextAsync(fileName);
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<GameHistory>(result);
                }
                catch
                {
                    return null;
                }
            }

            return null;
        }
    }


}
