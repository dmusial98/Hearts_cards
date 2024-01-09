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
			var lines = await GetTextFromFile();
			var history = new TxtFileLogReader("input.txt").GetGameHistoryAsync();


		}
	}
}
