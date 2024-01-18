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
		public void ReadHistoryFromExcelFile()
		{
			ExcelFileReader reader = new ExcelFileReader();
			var task = reader.GetGameHistoryAsync();
			task.Wait();

			Assert.IsNotNull(task.Result);
			Assert.AreEqual(2, task.Result.Rounds.Count);
			Assert.AreEqual("Five Heart", task.Result.Rounds[0].PlayerCardsBeforeExchange[0].Cards[0].ToString());
			Assert.AreEqual("Ace Spade", task.Result.Rounds[1].PlayerCardsBeforeExchange[0].Cards[0].ToString());

			//reader.WriteCardsBeforeTrickInExcelFile(@"LogFiles\Hearts_history.xlsx");

		}


		//[TestMethod]
		//public void RunExcelChange()
		//{
		//	ExcelFileReader reader = new ExcelFileReader();
		//	reader.ChangeNameColorsAndValuesInExcel(@"LogFiles\Hearts_history.xlsx");
		//}


	}
}
