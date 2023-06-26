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
			DeleteAllDocumentsInDirectory();
		}

		public async Task<string> GetTextFromFile()
		{
			var directory = new DirectoryInfo("LogFiles");
			var file = directory.GetFiles().OrderByDescending(f => f.LastWriteTime).First();

			using (var readTask = File.ReadAllTextAsync(file.FullName))
				return await readTask;
		}

		public void DeleteAllDocumentsInDirectory()
		{
			DirectoryInfo di = new DirectoryInfo("LogFiles");

			foreach (FileInfo file in di.GetFiles())
			{
				file.Delete();
			}
			foreach (DirectoryInfo dir in di.GetDirectories())
			{
				dir.Delete(true);
			}
		}

		[TestMethod]
		public async Task WriteUserConnected_CorrectOutput()
		{
			TxtFileLogWriter writer = new TxtFileLogWriter();

			string output = await WriteUserConnected_BaseTest(writer);
			string result = await GetTextFromFile();

			Assert.AreEqual(output, result);
		}

		[TestMethod]
		public async Task WriteUserClickedStartGame_CorrectOutput()
		{
			TxtFileLogWriter writer = new TxtFileLogWriter();

			string output = await WriteUserClickedStartGame_BaseTest(writer);
			string result = await GetTextFromFile();

			Assert.AreEqual(output, result);
		}

		[TestMethod]
		public async Task WriteStartGame_CorrectOutput()
		{
			TxtFileLogWriter writer = new TxtFileLogWriter();

			string output = await WriteStartGame_BaseTest(writer);
			string result = await GetTextFromFile();

			Assert.AreEqual(output, result);

		}

		[TestMethod]
		public async Task WritePlayersGotCards_CorrectOutput()
		{
			TxtFileLogWriter writer = new TxtFileLogWriter();

			string output = await WritePlayersGotCards_BaseTest(writer);
			string result = await GetTextFromFile();

			Assert.AreEqual(output, result);
		}

		[TestMethod]
		public async Task WritePlayerGaveCardsExchange_CorrectOutput()
		{
			TxtFileLogWriter writer = new TxtFileLogWriter();

			string output = await WritePlayerGaveCardsExchange_BaseTest(writer);
			string result = await GetTextFromFile();

			Assert.AreEqual(output, result);
		}

		[TestMethod]
		public async Task WritePlayerReceivedCardsExchange_CorrectOutput()
		{
			TxtFileLogWriter writer = new TxtFileLogWriter();

			string output = await WritePlayerReceivedCardsExchange_BaseTest(writer);
			string result = await GetTextFromFile();

			Assert.AreEqual(output, result);
		}

		[TestMethod]
		public async Task WritePlayerThrewCard_CorrectOutput()
		{
			TxtFileLogWriter writer = new TxtFileLogWriter();

			string output = await WritePlayerThrewCard_BaseTest(writer);
			string result = await GetTextFromFile();

			Assert.AreEqual(output, result);
		}

		[TestMethod]
		public async Task WriteTrick_CorrectOutput()
		{
			TxtFileLogWriter writer = new TxtFileLogWriter();

			string output = await WriteTrick_BaseTest(writer);
			string result = await GetTextFromFile();

			Assert.AreEqual(output, result);
		}

		[TestMethod]
		public async Task WritePlayersPointsInRound_CorrectOutput()
		{
			TxtFileLogWriter writer = new TxtFileLogWriter();

			string output = await WritePlayersPointsInRound_BaseTest(writer);
			string result = await GetTextFromFile();

			Assert.AreEqual(output, result);
		}

		[TestMethod]
		public async Task WritePlayersPointsAfterRound_CorrectOutput()
		{
			TxtFileLogWriter writer = new TxtFileLogWriter();

			string output = await WritePlayersPointsAfterRound_BaseTest(writer);
			string result = await GetTextFromFile();

			Assert.AreEqual(output, result);
		}

		[TestMethod]
		public async Task WritePlayersPlacesAfterGame_CorrectOutput()
		{
			TxtFileLogWriter writer = new TxtFileLogWriter();

			string output = await WritePlayersPlacesAfterGame_BaseTest(writer);
			string result = await GetTextFromFile();

			Assert.AreEqual(output, result);
		}

		[TestMethod]
		public async Task WritePlayersCards_CorrectOutput()
		{
			TxtFileLogWriter writer = new TxtFileLogWriter();

			string output = await WritePlayersCards_BaseTest(writer);
			string result = await GetTextFromFile();

			Assert.AreEqual(output, result);
		}

		[TestMethod]
		public async Task WriteClientSendMessage_CorrectOutput()
		{
			TxtFileLogWriter writer = new TxtFileLogWriter();

			string output = await WriteClientSendMessage_BaseTest(writer);
			string result = await GetTextFromFile();

			Assert.AreEqual(output, result);
		}

	}
}
