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


		public void ChangeNameColorsAndValuesInExcel(string fileName)
		{
			var fileInfo = new FileInfo(fileName);
			using (var package = new ExcelPackage(fileInfo))
			{
				var sheet = package.Workbook.Worksheets["Round_01"];

				var cells = sheet.Cells["B4:N7"];
				doForeach(cells);
				cells = sheet.Cells["B10:D13"];
				doForeach(cells);
				cells = sheet.Cells["B17:N20"];
				doForeach(cells);
				cells = sheet.Cells["B25:E37"];
				doForeach(cells);

				// Save to file
				package.Save();
			}
		}

		private void doForeach(ExcelRange cells)
		{
			foreach (var cell in cells)
			{
				string value = cell.Text;

				cell.Value = cell.Text.Replace("kier", "Heart");
				cell.Value = cell.Text.Replace("trefl", "Club");
				cell.Value = cell.Text.Replace("karo", "Diamond");
				cell.Value = cell.Text.Replace("pik", "Spade");

				string[] splitted = cell.Text.Split(' ');

				cell.Value = splitted[0] switch
				{
					"2" => "Two" + " " + splitted[1],
					"3" => "Three" + " " + splitted[1],
					"4" => "Four" + " " + splitted[1],
					"5" => "Five" + " " + splitted[1],
					"6" => "Six" + " " + splitted[1],
					"7" => "Seven" + " " + splitted[1],
					"8" => "Eight" + " " + splitted[1],
					"9" => "Nine" + " " + splitted[1],
					"10" => "Ten" + " " + splitted[1],
					"J" => "Jack" + " " + splitted[1],
					"Q" => "Queen" + " " + splitted[1],
					"K" => "King" + " " + splitted[1],
					"A" => "Ace" + " " + splitted[1],
					_ => cell.Value
				};
			}
		}

	}
}
