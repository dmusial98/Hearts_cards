using Hearts_server.GameLogic;
using Hearts_server.GameLogic.Cards;
using HeartsServer.GameLogic.Shuffle;
using HeartsServer.ResultsWriterReader;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeartsServer.GameLogic.Tests.Writers.Tests.ILogWriter.Tests
{
	[TestClass]
	public class ConsoleLogWriterTest : LogWriterTestBase
	{
		public static StringWriter stringWriter;
		public static TextWriter standardOut;
		public static ConsoleLogWriter writer;

		[ClassInitialize()]
		public static void ClassInitialize(TestContext testContext)
		{
			writer = new ConsoleLogWriter();
			standardOut = Console.Out;
			stringWriter = new StringWriter();
		}

		[ClassCleanup()]
		public static void ClassCleanup()
		{
			Console.SetOut(standardOut);
		}

		[TestCleanup()]
		public void TestCleanup()
		{
			Player.ResetIdCounter();
			stringWriter.GetStringBuilder().Clear();
		}

		[TestInitialize()]
		public void TestInitialize()
		{
			Console.SetOut(stringWriter);
		}

		[TestMethod]
		public async Task WriteUserConnected_CorrectOutput()
		{
			string output = await WriteUserConnected_BaseTest(writer);

			Assert.AreEqual(output, stringWriter.ToString());
		}

		[TestMethod]
		public async Task WriteUserClickedStartGame_CorrectOutput()
		{
			string output = await WriteUserClickedStartGame_BaseTest(writer);

			Assert.AreEqual(output, stringWriter.ToString());
		}

		[TestMethod]
		public async Task WriteStartGame_CorrectOutput()
		{
			string output = await WriteStartGame_BaseTest(writer);

			Assert.AreEqual(output, stringWriter.ToString());
		}

		[TestMethod]
		public async Task WritePlayersGotCards_CorrectOutput()
		{
			string output = await WritePlayersGotCards_BaseTest(writer);

			Assert.AreEqual(output.ToString(), stringWriter.ToString());
		}

		[TestMethod]
		public async Task WritePlayerGaveCardsExchangeLog_CorrectOutput()
		{
			string output = await WritePlayerGaveCardsExchange_BaseTest(writer);

			Assert.AreEqual(output, stringWriter.ToString());
		}

		[TestMethod]
		public async Task WritePlayerReceivedCardsExchangeLog_CorrectOutput()
		{
			string output = await WritePlayerReceivedCardsExchange_BaseTest(writer);

			Assert.AreEqual(output, stringWriter.ToString());
		}

		[TestMethod]
		public async Task WritePlayerThrewCardLog_CorrectOutput()
		{
			string output = await WritePlayerThrewCard_BaseTest(writer);

			Assert.AreEqual(output, stringWriter.ToString());
		}

		[TestMethod]
		public async Task WriteTrick_CorrectOutput()
		{
			string output = await WriteTrick_BaseTest(writer);

			Assert.AreEqual(output, stringWriter.ToString());
		}

		[TestMethod]
		public async Task WritePlayersPointsInRound_CorrectOutput()
		{
			string output = await WritePlayersPointsInRound_BaseTest(writer);

			Assert.AreEqual(output.ToString(), stringWriter.ToString());
		}

		[TestMethod]
		public async Task WritePlayersPointsAfterRound_CorrectOutput()
		{
			string output = await WritePlayersPointsAfterRound_BaseTest(writer);

			Assert.AreEqual(output.ToString(), stringWriter.ToString());
		}

		[TestMethod]
		public async Task WritePlayersPlacesAfterGame_CorrectOutput()
		{
			string output = await WritePlayersPlacesAfterGame_BaseTest(writer);

			Assert.AreEqual(output.ToString(), stringWriter.ToString());
		}

		[TestMethod]
		public async Task WritePlayersCards_CorrectOutput()
		{
			string output = await WritePlayersCards_BaseTest(writer);

			Assert.AreEqual(output.ToString(), stringWriter.ToString());
		}

		[TestMethod]
		public async Task WriteClientSendMessage_CorrectOutput()
		{
		string output = await WriteClientSendMessage_BaseTest(writer);

			Assert.AreEqual(output, stringWriter.ToString());
		}

		[TestMethod]
		public async Task WriteRoundStarted_CorrectOutput()
		{
			string output = await WriteRoundStarted_BaseTest(writer);

			Assert.AreEqual(output, stringWriter.ToString());
		}
	}
}
