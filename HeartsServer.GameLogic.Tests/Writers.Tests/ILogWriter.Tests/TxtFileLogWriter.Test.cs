using Hearts_server.GameLogic;
using HeartsServer.ResultsWriterReader;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeartsServer.GameLogic.Tests.Writers.Tests.ILogWriter.Tests
{
    [TestClass]
    public class TxtFileLogWriterTest : LogWriterTestBase
    {

        [TestCleanup()]
        public void TestCleanup()
        {
            Player.ResetIdCounter();

        }

        public async Task<string> GetTextFromFile()
        {
            var directory = new DirectoryInfo("LogFiles");
            var file = directory.GetFiles().OrderByDescending(f => f.LastWriteTime).First();

            using (var readTask = File.ReadAllTextAsync(file.FullName))
                return await readTask;
        }

        //TODO przeniesc do klasy bazowej
        [TestMethod]
        public async Task WriteUserConnected_CorrectOutput()
        {
            Player player = new("John");
            TxtFileLogWriter writer = new TxtFileLogWriter();
            await writer.WriteUserConnectedAsync(player);

            string result = await GetTextFromFile();

            Assert.AreEqual(String.Concat("C1 ", DateTime.Now.ToString("G"), ":  player ", player.Name, ", ID: ", player.Id, " connected with server\r\n"), result);
        }
    }
}
