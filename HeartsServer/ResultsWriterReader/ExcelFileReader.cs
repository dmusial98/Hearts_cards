using Hearts_server.ResultsWriterReader;
using HeartsServer.GameLogic.History;
using OfficeOpenXml;

namespace HeartsServer.ResultsWriterReader
{
	public class ExcelFileReader : GameFileReader, IGameReader
	{
		public override async Task<GameHistory> GetGameHistoryAsync()
		{
			GameHistory gameHistory = new GameHistory();
			var file = GetFile();

			return gameHistory;
		}


		public void ChangeNameColorsInExcel(string fileName)
		{
			using (var package = new ExcelPackage(@"c:\temp\myWorkbook.xlsx"))
			{
				var sheet = package.Workbook.Worksheets.Add("Round_01");

				var cells = sheet.Cells["B4:N7"];

				foreach (var cell in cells)
				{
					string value = cell.Text;

					string[] arr = value.Split(' ');

					value.Replace("kier", "Heart");
					value.Replace("trefl", "Club");
					value.Replace("karo", "Diamond");
					value.Replace("pik", "Spade");
				}

				// Save to file
				package.Save();
			}
		}
	}
}
