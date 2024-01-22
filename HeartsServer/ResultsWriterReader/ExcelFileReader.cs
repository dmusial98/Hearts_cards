using Hearts_server.GameLogic;
using Hearts_server.GameLogic.Cards;
using Hearts_server.ResultsWriterReader;
using HeartsServer.GameLogic.Consts;
using HeartsServer.GameLogic.History;
using Microsoft.AspNetCore.Mvc.Formatters;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;

namespace HeartsServer.ResultsWriterReader
{
    public class ExcelFileReader : GameFileReader, IGameReader
    {
        public ExcelFileReader(string fileName) : base(fileName) { }
        public ExcelFileReader() : base() {}

        private GameHistory gameHistory { get; set; }

        //TODO: posprzatac w kodzie
        public override async Task<GameHistory> GetGameHistoryAsync()
        {
            gameHistory = new GameHistory();
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
                startTime = DateTime.MinValue;
                //TODO: dodaj error do listy bledow w historii gry}
            }

            gameHistory.StartTime = startTime;
            gameHistory.Players = GetPlayerHistory(firstSheet);
            gameHistory.IsTerminated = firstSheet.Cells["H1"].Text == "tak";

            gameHistory.Rounds = GetRoundHistory(package);

            var toReturn = gameHistory;
            gameHistory = null;
            return toReturn;
        }

        private List<RoundHistory> GetRoundHistory(ExcelPackage package)
        {
            List<RoundHistory> list = [];

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
                    roundHistory.PlayersAfterRound = GetPlayerHistory(sheet);

                if (!String.IsNullOrEmpty(sheet.Cells["B11"].Text)) //cards before exchange
                    roundHistory.PlayerCardsBeforeExchange = GetPlayerCardsHistory(sheet, firstRow: (11, 2), cardsNumber: NumbersConsts.CARDS_FOR_PLAYER_CONST);

                if (!String.IsNullOrEmpty(sheet.Cells["B17"].Text)) //exchange
                    roundHistory.Exchange = GetExchangeHistory(sheet);

                if (!String.IsNullOrEmpty(sheet.Cells["B24"].Text)) //cards after exchange
                    roundHistory.PlayerCardsAfterExchange = GetPlayerCardsHistory(sheet, firstRow: (24, 2), cardsNumber: NumbersConsts.CARDS_FOR_PLAYER_CONST);

                if (!String.IsNullOrEmpty(sheet.Cells["B32"].Text)) //tricks
                    roundHistory.Tricks = GetTricksHistory(sheet);

                list.Add(roundHistory);
            }
            return list;
        }

        private List<PlayerHistory> GetPlayerHistory(ExcelWorksheet sheet)
        {
            List<PlayerHistory> history = [];

            for (int i = 0; i < NumbersConsts.PLAYERS_NUMBER_CONST; i++)
            {
                var playerHistory = new PlayerHistory
                {
                    PlayerId = i + 1,
                    Name = sheet.Cells[4, i + 2].Text,
                    PointsInGame = int.TryParse(sheet.Cells[5, i + 2].Text, out int playerPointsAfterRound) ? playerPointsAfterRound : -1,
                    Place = int.TryParse(sheet.Cells[6, i + 2].Text, out int placeNumberFirst) ? placeNumberFirst : -1,
                    Bonuses = int.TryParse(sheet.Cells[7, i + 2].Text, out int playerBonusesNumber) ? playerBonusesNumber : -1,
                };

                history.Add(playerHistory);
            }

            return history;
        }

        private List<PlayerCardsHistory> GetPlayerCardsHistory(ExcelWorksheet sheet, (int, int) firstRow, int cardsNumber)
        {
            List<PlayerCardsHistory> history = [];
            for (int i = 0; i < NumbersConsts.PLAYERS_NUMBER_CONST; i++)
            {
                ExcelRange range = sheet.Cells[firstRow.Item1 + i, firstRow.Item2, firstRow.Item1, firstRow.Item2 + cardsNumber - 1];
                history.Add(new PlayerCardsHistory
                {
                    PlayerId = i + 1,
                    PlayerName = gameHistory.Players.Where(p => p.PlayerId == i + 1).First().Name,
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

                var card = GetSingleCard(cellStr);
                if (card != null)
                    list.Add(card);
            }

            return list;
        }

        private Card GetSingleCard(string input)
        {
            if (String.IsNullOrEmpty(input))
                return null;

            var cardStr = input.Split(' ');
            if (cardStr.Length != 2)
                return null;

            CardValue value = (CardValue)Enum.Parse(typeof(CardValue), cardStr[0]);
            CardColour colour = (CardColour)Enum.Parse(typeof(CardColour), cardStr[1]);

            var card = Game.Instance.GetCard(value, colour);
            if (card != null)
                return card;

            return null;
        }

        private List<TrickHistory> GetTricksHistory(ExcelWorksheet sheet)
        {
            List<TrickHistory> list = new List<TrickHistory>();
            for (int i = 0; i < NumbersConsts.TRICKS_NUMBER_IN_ROUND_CONST; i++)
            {
                var wereCards = sheet.Cells[32 + i, 2].Text;
                if (!String.IsNullOrEmpty(wereCards))
                {
                    list.Add(new TrickHistory
                    {
                        TrickNumber = i + 1,
                        WhoWonId = int.TryParse(sheet.Cells[32 + i, 7].Text, out int whoWonInt) ? whoWonInt : -1,
                        WhoStartedId = int.TryParse(sheet.Cells[32 + i, 6].Text, out int whoStartedInt) ? whoStartedInt : -1,
                        Queue = new List<QueueHistory>
                        {
                            new QueueHistory
                            {
                                PlayerId = 1,
                                PlayerName = gameHistory.Players.Where(p =>p.PlayerId == 1).First().Name,
                                Card = GetSingleCard(sheet.Cells[32 + i, 2].Text)
                            },
                            new QueueHistory
                            {
                                PlayerId = 2,
                                PlayerName = gameHistory.Players.Where(p =>p.PlayerId == 2).First().Name,
                                Card = GetSingleCard(sheet.Cells[32 + i, 3].Text)
                            },
                            new QueueHistory
                            {
                                PlayerId = 3,
                                PlayerName = gameHistory.Players.Where(p =>p.PlayerId == 3).First().Name,
                                Card = GetSingleCard(sheet.Cells[32 + i, 4].Text)
                            },
                            new QueueHistory
                            {
                                PlayerId = 4,
                                PlayerName = gameHistory.Players.Where(p =>p.PlayerId == 4).First().Name,
                                Card = GetSingleCard(sheet.Cells[32 + i, 5].Text)
                            }
                        },
                        PointsAfterTrick = GetPlayersPointsAfterTrick(sheet.Cells[32 + i, 9, 32 + i, 12]),

                        PlayerCardsBeforeTrick = new List<PlayerCardsHistory>
                        {
                            new PlayerCardsHistory
                            {
                                PlayerId = 1,
                                PlayerName = gameHistory.Players.Where(p=> p.PlayerId == 1).First().Name,
                                Cards = GetCardsList(sheet.Cells[48 + i, 2, 48 + i, 14])
                            },
                            new PlayerCardsHistory
                            {
                                PlayerId = 2,
                                PlayerName = gameHistory.Players.Where(p=> p.PlayerId == 2).First().Name,
                                Cards = GetCardsList(sheet.Cells[48 + i, 15, 48 + i, 27])
                            },
                            new PlayerCardsHistory
                            {
                                PlayerId = 3,
                                PlayerName = gameHistory.Players.Where(p=> p.PlayerId == 3).First().Name,
                                Cards = GetCardsList(sheet.Cells[48 + i, 28, 48 + i, 40])
                            },
                            new PlayerCardsHistory
                            {
                                PlayerId = 4,
                                PlayerName = gameHistory.Players.Where(p=> p.PlayerId == 4).First().Name,
                                Cards = GetCardsList(sheet.Cells[48 + i, 41, 48 + i, 53])
                            }
                        }
                    });
                }
            }


            return list;
        }

        private List<PlayerHistory> GetPlayersPointsAfterTrick(ExcelRange cells)
        {
            List<PlayerHistory> list = [];
            int playerIndex = 1;
            foreach (var cell in cells)
            {
                var result = int.TryParse(cell.Text, out int points);
                if (result && points >= 0 && points < 126)
                {
                    list.Add(new PlayerHistory
                    {
                        PlayerId = playerIndex,
                        Name = gameHistory.Players.Where(p => p.PlayerId == playerIndex).First().Name,
                        PointsInRound = points,
                    });
                    playerIndex++;
                }
            }

            return list;
        }

        //TODO: przeniesc do pliku z zapisem excela, a nie odczytem
        public void WriteCardsBeforeTrickInExcelFile(string fileName)
        {
            var fileInfo = new FileInfo(fileName);
            using var package = new ExcelPackage(fileInfo);

            var sheet = package.Workbook.Worksheets["Round_01"];

            List<Card>[] playersCards = new List<Card>[4];

            playersCards[0] = GetCardsList(sheet.Cells["B24:N24"]);
            playersCards[1] = GetCardsList(sheet.Cells["B25:N25"]);
            playersCards[2] = GetCardsList(sheet.Cells["B26:N26"]);
            playersCards[3] = GetCardsList(sheet.Cells["B27:N27"]);

            var playersThrewCards = new List<Card>[4];
            playersThrewCards[0] = GetCardsList(sheet.Cells["B32:B44"]);
            playersThrewCards[1] = GetCardsList(sheet.Cells["C32:C44"]);
            playersThrewCards[2] = GetCardsList(sheet.Cells["D32:D44"]);
            playersThrewCards[3] = GetCardsList(sheet.Cells["E32:E44"]);

            CleanCells(sheet.Cells["B48:BA60"]);


            for (int i = 0; i < NumbersConsts.TRICKS_NUMBER_IN_ROUND_CONST; i++)
            {

                for (int j = 0; j < NumbersConsts.PLAYERS_NUMBER_CONST; j++)
                {
                    int cardIndex = 0;
                    foreach (var card in playersCards[j])
                    {
                        //zapisz do excela w wierszu wszystkie karty gracza
                        sheet.Cells[48 + i, 2 + j * 13 + cardIndex].Value = card.ToString();
                        cardIndex++;
                    }
                    //usun z listy pozostalych kart te pierwsza
                    playersCards[j].Remove(playersThrewCards[j][0]);

                    //usun z listy rzuconych kart pierwsza
                    playersThrewCards[j].RemoveAt(0);
                }
            }
            package.Save();

        }

        //TODO: przeniesc do pliku z zapisem excela, a nie odczytem
        private void CleanCells(ExcelRange cells)
        {
            foreach (var cell in cells)
            {
                cell.Value = "";
            }
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
