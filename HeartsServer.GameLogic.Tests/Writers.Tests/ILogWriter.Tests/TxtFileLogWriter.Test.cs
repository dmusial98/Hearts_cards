﻿using Hearts_server.GameLogic;
using HeartsServer.ResultsWriterReader;

namespace HeartsServer.GameLogic.Tests.Writers.Tests.ILogWriter.Tests
{
    [TestClass]
    public class TxtFileLogWriterTest : LogWriterTestBase
    {
        private bool _fileToClean = true;
        private string _path = Path.Combine("LogFiles", "txtFileLogWriter.Test", "temp.txt");

        [TestInitialize]
        public void TestInitialize()
        {
            File.WriteAllText(_path, "");
            _fileToClean = true;
        }

        [TestCleanup()]
        public void TestCleanup()
        {
            Player.ResetIdCounter();
            DeleteFilesInRirectory();
        }

        public async Task<string> GetTextFromFile()
        {
            var directory = new DirectoryInfo(Path.Combine("LogFiles", "txtFileLogWriter.Test"));
            var file = directory.GetFiles().OrderByDescending(f => f.LastWriteTime).First();

            using (var readTask = File.ReadAllTextAsync(file.FullName))
                return await readTask;
        }

        public void DeleteFilesInRirectory()
        {
            DirectoryInfo directory = new DirectoryInfo(Path.Combine("LogFiles", "txtFileLogWriter.Test"));
            var files = directory.GetFiles();
                
            foreach (var file in files)
                if (_fileToClean)
                    file.Delete();
        }

        [TestMethod]
        public async Task WriteUserConnected_CorrectOutput()
        {
            TxtFileLogWriter writer = new TxtFileLogWriter(_path);

            string output = await WriteUserConnected_BaseTest(writer);
            string result = await GetTextFromFile();

            Assert.AreEqual(output, result);
        }

        [TestMethod]
        public async Task WriteUserClickedStartGame_CorrectOutput()
        {
            TxtFileLogWriter writer = new TxtFileLogWriter(_path);

            string output = await WriteUserClickedStartGame_BaseTest(writer);
            string result = await GetTextFromFile();

            Assert.AreEqual(output, result);
        }

        [TestMethod]
        public async Task WriteStartGame_CorrectOutput()
        {
            TxtFileLogWriter writer = new TxtFileLogWriter(_path);

            string output = await WriteStartGame_BaseTest(writer);
            string result = await GetTextFromFile();

            Assert.AreEqual(output, result);
        }

        [TestMethod]
        public async Task WritePlayersGotCards_CorrectOutput()
        {
            TxtFileLogWriter writer = new TxtFileLogWriter(_path);

            string output = await WritePlayersGotCards_BaseTest(writer);
            string result = await GetTextFromFile();

            Assert.AreEqual(output, result);
        }

        [TestMethod]
        public async Task WritePlayerGaveCardsExchange_CorrectOutput()
        {
            TxtFileLogWriter writer = new TxtFileLogWriter(_path);

            string output = await WritePlayerGaveCardsExchange_BaseTest(writer);
            string result = await GetTextFromFile();

            Assert.AreEqual(output, result);
        }

        [TestMethod]
        public async Task WritePlayerReceivedCardsExchange_CorrectOutput()
        {
            TxtFileLogWriter writer = new TxtFileLogWriter(_path);

            string output = await WritePlayerReceivedCardsExchange_BaseTest(writer);
            string result = await GetTextFromFile();

            Assert.AreEqual(output, result);
        }

        [TestMethod]
        public async Task WritePlayerThrewCard_CorrectOutput()
        {
            TxtFileLogWriter writer = new TxtFileLogWriter(_path);

            string output = await WritePlayerThrewCard_BaseTest(writer);
            string result = await GetTextFromFile();

            Assert.AreEqual(output, result);
        }

        [TestMethod]
        public async Task WriteTrick_CorrectOutput()
        {
            TxtFileLogWriter writer = new TxtFileLogWriter(_path);

            string output = await WriteTrick_BaseTest(writer);
            string result = await GetTextFromFile();

            Assert.AreEqual(output, result);
        }

        [TestMethod]
        public async Task WritePlayersPointsInRound_CorrectOutput()
        {
            TxtFileLogWriter writer = new TxtFileLogWriter(_path);

            string output = await WritePlayersPointsInRound_BaseTest(writer);
            string result = await GetTextFromFile();

            Assert.AreEqual(output, result);
        }

        [TestMethod]
        public async Task WritePlayersPointsAfterRound_CorrectOutput()
        {
            TxtFileLogWriter writer = new TxtFileLogWriter(_path);

            string output = await WritePlayersPointsAfterRound_BaseTest(writer);
            string result = await GetTextFromFile();

            Assert.AreEqual(output, result);
        }

        [TestMethod]
        public async Task WritePlayersPlacesAfterGame_CorrectOutput()
        {
            TxtFileLogWriter writer = new TxtFileLogWriter(_path);

            string output = await WritePlayersPlacesAfterGame_BaseTest(writer);
            string result = await GetTextFromFile();

            Assert.AreEqual(output, result);
        }

        [TestMethod]
        public async Task WritePlayersCards_CorrectOutput()
        {
            TxtFileLogWriter writer = new TxtFileLogWriter(_path);

            string output = await WritePlayersCards_BaseTest(writer);
            string result = await GetTextFromFile();

            Assert.AreEqual(output, result);
        }

        [TestMethod]
        public async Task WriteClientSendMessage_CorrectOutput()
        {
            TxtFileLogWriter writer = new TxtFileLogWriter(_path);

            string output = await WriteClientSendMessage_BaseTest(writer);
            string result = await GetTextFromFile();

            Assert.AreEqual(output, result);
        }

        [TestMethod]
        public async Task WriteRoundStarted_CorrectOutput()
        {
            TxtFileLogWriter writer = new TxtFileLogWriter(_path);

            string output = await WriteRoundStarted_BaseTest(writer);
            string result = await GetTextFromFile();

            Assert.AreEqual(output, result);
        }

        [TestMethod]
        public async Task WriteTrickStarted_CorrectOutput()
        {
            TxtFileLogWriter writer = new TxtFileLogWriter(_path);

            string output = await WriteTrickStarted_BaseTest(writer);
            string result = await GetTextFromFile();

            Assert.AreEqual(output, result);
        }

        [TestMethod]
        public async Task AppendTextToExistingFile_TextIsInFile()
        {
            Player player = new("John"), player2 = new("Michael");
            string message = "message from user ;)", message2 = "second message from user";

            var date = DateTime.Now.ToString(Consts.LogCodesConsts.POLISH_CULTURE_INFO);
            TxtFileLogWriter writer = new(_path);
            await writer.WriteClientSendMessageAsync(player, message);
            await writer.WriteClientSendMessageAsync(player2, message2);
            await writer.WriteUserClickedStartGameAsync(player);
            string result = await GetTextFromFile();

            Assert.IsTrue(result.Contains(String.Concat("C13 ", date, ":  player ", player.Name, ", ID: ",
                player.Id.ToString(), " send message: ", message, Consts.LogCodesConsts.NEW_LINE)));
            Assert.IsTrue(result.Contains(String.Concat("C13 ", date, ":  player ", player2.Name, ", ID: ",
                player2.Id.ToString(), " send message: ", message2, Consts.LogCodesConsts.NEW_LINE)));
            Assert.IsTrue(result.Contains(String.Concat("C2 ", date, ":  player ", player.Name, ", ID: ", player.Id,
                " clicked start game", Consts.LogCodesConsts.NEW_LINE)));
        }

        [TestMethod]
        public async Task WriteTxtFileFromGameHistory()
        {
            var history =
                await JsonFileReaderWriter.ReadGameHistory(Path.Combine("LogFiles",
                    "17.06.2025_20_58_00_history.json"));
            Assert.IsNotNull(history);
        
            await new TxtFileLogWriter(history.StartTime).SaveTxtFileLogFileFromGameHistory(history);
            _fileToClean = false;
        }
    }
}