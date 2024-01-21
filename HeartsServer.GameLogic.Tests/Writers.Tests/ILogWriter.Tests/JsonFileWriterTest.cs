using HeartsServer.ResultsWriterReader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeartsServer.GameLogic.Tests.Writers.Tests.ILogWriter.Tests
{
    [TestClass]
    public class JsonFileWriterTest
    {
        [TestMethod]
        public async Task WriteJsonFile()
        {
            ExcelFileReader reader = new ExcelFileReader();
            var history = await reader.GetGameHistoryAsync();
            await JsonFileReaderWriter.WriteGameHistory(history);
        }

        [TestMethod]
        public async Task ReadJsonFile()
        {
            var history = await JsonFileReaderWriter.ReadGameHistory(@"LogFiles\15.01.2024_20_58_00_history.json");
            Assert.IsNotNull(history);


        }

    }
}
