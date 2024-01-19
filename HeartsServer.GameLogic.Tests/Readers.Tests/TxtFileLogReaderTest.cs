using HeartsServer.ResultsWriterReader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeartsServer.GameLogic.Tests.Readers.Tests
{
	[TestClass]
	public class TxtFileLogReaderTest
	{
		[TestMethod]
		public async Task<string[]> GetTextFromFile()
		{
			var directory = new DirectoryInfo("LogFiles");
			var file = directory.GetFiles().OrderByDescending(f => f.LastWriteTime).First();

			using (var readTask = File.ReadAllLinesAsync(file.FullName))
				return await readTask;
		}

        [TestMethod]
        public async Task GetGameHistoryFromTxtLogFile()
        {
			//var history = 


            var history = await new TxtFileLogReader("15.01.2024_20_58_00_logs.txt").GetGameHistoryAsync();

            Assert.IsNotNull(history);

            await JsonFileReaderWriter.WriteGameHistory(history);

			//TODO: punkty w grze przy podsumowaniu graczy na poczatku
        }
    }
}
