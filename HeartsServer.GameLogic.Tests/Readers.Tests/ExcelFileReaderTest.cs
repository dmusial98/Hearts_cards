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
		public void RunExcelChange()
		{
			ExcelFileReader reader = new ExcelFileReader();
			reader.ChangeNameColorsAndValuesInExcel(@"LogFiles\Hearts_history.xlsx");


		}
	}
}
