using HeartsServer.ResultsWriterReader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeartsServer.GameLogic.Tests.Readers.Tests
{
	[TestClass]
	public class ExcelFileReaderTest
	{
		[TestMethod]
		public async Task ReadHistoryFromExcelFile()
		{
			ExcelFileReader reader = new ExcelFileReader();
			var history = await reader.GetGameHistoryAsync();

			Assert.IsNotNull(history);
			Assert.AreEqual(2, history.Rounds.Count);
			Assert.AreEqual("Five Heart", history.Rounds[0].PlayerCardsBeforeExchange[0].Cards[0].ToString());
			Assert.AreEqual("Ace Spade", history.Rounds[1].PlayerCardsBeforeExchange[0].Cards[0].ToString());

			await JsonFileReaderWriter.WriteGameHistory(history);


			//reader.WriteCardsBeforeTrickInExcelFile(@"LogFiles\Hearts_history.xlsx");

		}

		//[TestMethod]
		//public async Task Sav


		//[TestMethod]
		//public void RunExcelChange()
		//{
		//	ExcelFileReader reader = new ExcelFileReader();
		//	reader.ChangeNameColorsAndValuesInExcel(@"LogFiles\Hearts_history.xlsx");
		//}


	}
}
