using Hearts_server.GameLogic;
using Hearts_server.GameLogic.Cards;
using Hearts_server.ResultsWriterReader;
using HeartsServer.GameLogic.Consts;
using HeartsServer.GameLogic.History;
using OfficeOpenXml;
using System.Globalization;

namespace HeartsServer.ResultsWriterReader
{
    public class ExcelFileReader : GameFileReader, IGameReader
    {
        //TODO ogarnac w excelu testowym i w funkcji wczytujacej karty graczy po lewie
        public override async Task<GameHistory> GetGameHistoryAsync()
        {
            GameHistory gameHistory = new GameHistory();
            var file = GetFile();

            using var package = new ExcelPackage(file);
            var firstSheet = package.Workbook.Worksheets["Round_01"];

            DateTime startTime = new DateTime();
            try
            {
                startTime = DateTime.ParseExact(firstSheet.Cells["E1"].Text, "dd.MM.yyyy_HH.mm", CultureInfo.InvariantCulture);
            }
            catch
            {
                //TODO: dodaj error do listy bledow w historii gry}
            }

            gameHistory.StartTime = startTime;
            gameHistory.Players = GetPlayerHistory(firstSheet);
            gameHistory.IsTerminated = firstSheet.Cells["H1"].Text == "tak";

            foreach (var sheet in package.Workbook.Worksheets.Where(w => w.Name.ToLower().Contains("round")))
            {
                RoundHistory roundHistory = new RoundHistory();
                if (int.TryParse(sheet.Cells["B1"].Text, out int roundNumberInt))
                    roundHistory.RoundNumber = roundNumberInt;
                else
                {
                    //TODO: dodaj error do listy w grze
                    continue;
                }

                if (!String.IsNullOrEmpty(sheet.Cells["B5"].Text)) //player history
                    roundHistory.PlayerAfterRound = GetPlayerHistory(sheet);

                if (!String.IsNullOrEmpty(sheet.Cells["B11"].Text)) //cards before exchange
                    roundHistory.PlayerCardsBeforeExchange = GetPlayerCardsHistory(sheet, firstRow: (11, 2), cardsNumber: NumbersConsts.CARDS_FOR_PLAYER_CONST, playersVertically: true);

                if (!String.IsNullOrEmpty(sheet.Cells["B17"].Text)) //exchange
                    roundHistory.Exchange = GetExchangeHistory(sheet);

                if (!String.IsNullOrEmpty(sheet.Cells["B24"].Text))
                    roundHistory.PlayerCardsAfterExchange = GetPlayerCardsHistory(sheet, firstRow: (24, 2), cardsNumber: NumbersConsts.CARDS_FOR_PLAYER_CONST, playersVertically: true);

                //TODO: tricks

                gameHistory.Rounds.Add(roundHistory);
            } //round









            return gameHistory;
        }

        private List<PlayerHistory> GetPlayerHistory(ExcelWorksheet sheet)
        {
            List<PlayerHistory> history = [];

            for (int i = 0; i < NumbersConsts.PLAYERS_NUMBER_CONST; i++)
            {
                var playerHistory = new PlayerHistory
                {
                    Id = i + 1,
                    Name = sheet.Cells[4, i + 2].Text,
                    Points = int.TryParse(sheet.Cells[5, i + 2].Text, out int playerPointsAfterRound) ? playerPointsAfterRound : -1,
                    Place = int.TryParse(sheet.Cells[6, i + 2].Text, out int placeNumberFirst) ? placeNumberFirst : -1,
                    Bonuses = int.TryParse(sheet.Cells[7, i + 2].Text, out int playerBonusesNumber) ? playerBonusesNumber : -1,
                };

                history.Add(playerHistory);
            }

            return history;
        }

        private List<PlayerCardsHistory> GetPlayerCardsHistory(ExcelWorksheet sheet, (int, int) firstRow, int cardsNumber, bool playersVertically)
        {
            List<PlayerCardsHistory> history = [];
            for (int i = 0; i < NumbersConsts.PLAYERS_NUMBER_CONST; i++)
            {
                ExcelRange range = null;
                if (playersVertically)
                    range = sheet.Cells[firstRow.Item1 + i, firstRow.Item2, firstRow.Item1, firstRow.Item2 + cardsNumber - 1];
                else //horizontally
                    range = sheet.Cells[firstRow.Item1, firstRow.Item2 + i * cardsNumber, firstRow.Item1, firstRow.Item2 + i * cardsNumber - 1];

                history.Add(new PlayerCardsHistory
                {
                    PlayerId = i + 1,
                    Cards = GetCardsList(range)
                });
            }

            return history;
        }

        private List<ExchangeHistory> GetExchangeHistory(ExcelWorksheet sheet)
        {
            var history = new List<ExchangeHistory>();
            for (int i = 0; i < NumbersConsts.PLAYERS_NUMBER_CONST; i++)
            {
                int playerId = i + 1;
                int fromWho = sheet.Cells["E17:E20"].FirstOrDefault(c => c.Text.Contains(playerId.ToString())).Start.Row - 16;

                history.Add(new ExchangeHistory
                {
                    IdPlayer = playerId,
                    ToWho = int.TryParse(sheet.Cells[17 + i, 5].Text, out int toWhoInt) ? toWhoInt : -1,
                    FromWho = fromWho,
                    Gave = GetCardsList(sheet.Cells[17 + i, 2, 17 + i, 4]),
                    Received = GetCardsList(sheet.Cells[17 + fromWho - 1, 2, 17 + fromWho - 1, 4]),
                });
            }

            return history;
        }

        private List<Card> GetCardsList(ExcelRange cells)
        {
            List<Card> list = [];

            foreach (var cell in cells)
            {
                var cellStr = cell.Text;
                if (String.IsNullOrEmpty(cellStr))
                    continue;

                var cardStr = cellStr.Split(' ');
                CardValue value = (CardValue)Enum.Parse(typeof(CardValue), cardStr[0]);
                CardColour colour = (CardColour)Enum.Parse(typeof(CardColour), cardStr[1]);

                var card = Game.Instance.GetCard(value, colour);
                if (card != null)
                    list.Add(card);
            }

            return list;
        }

        //TODO: przeniesc do pliku z zapisem excela, a nie odczytem
        public void ChangeNameColorsAndValuesInExcel(string fileName)
        {
            var fileInfo = new FileInfo(fileName);
            using var package = new ExcelPackage(fileInfo);

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

        //TODO: przeniesc do pliku z zapisem excela, a nie odczytem
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
