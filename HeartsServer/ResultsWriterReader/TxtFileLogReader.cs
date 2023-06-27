using Hearts_server.ResultsWriterReader;
using HeartsServer.GameLogic.History;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace HeartsServer.ResultsWriterReader
{
    public class TxtFileLogReader : IGameReader
    {
        public string FileName { get; set; } = string.Empty;

        public TxtFileLogReader(string fileName) => FileName = fileName;
        public TxtFileLogReader() { }

        public async Task<GameHistory> GetGameHistoryFromFileAsync()
        {
            DirectoryInfo directory = new DirectoryInfo("LogFiles");
            FileInfo file = null;
            if (FileName == string.Empty)
                file = directory.GetFiles().OrderByDescending(f => f.LastWriteTime).First();
            else
                file = directory.GetFiles().FirstOrDefault(f => f.Name == FileName);

            if (file == null)
                throw new FileNotFoundException();

            GameHistory history = new GameHistory();
            string[] linesFromFile = await LoadFromFile(file);
            var listLines = linesFromFile.ToList();
            
            //usun niepotrzebne logi +
            //zaladuj start gry C3 + 
            //zaladuj rozdanie kart C4 
            //zaladuj zaladuj wymianki C5 C6
            //zaladuj paczke czterech rzuconych kart, wygranie lewy i punkty w rundzie C7 C8 C9
            //zaladuj karty graczy po lewie C12
            //po 13 lewach zaladuj punkty po rundzie C10
            //powtarzaj aż załaduje informację o miejscach C11

            listLines.RemoveAll(IsUnnecessaryLog);

            var startGameLog = listLines.Where(s => s[..3] == "C3 ").ToArray();
            if (startGameLog.Count() != 1)
                throw new FileLoadException($"File {file.Name} doesn't contain exactly one start game message log");

            listLines.Remove(startGameLog.First());

            var playerCardsBeforeGameLog = listLines.Where(s => s[..3] == "C4 ").ToArray();

            


            return history;
        }

        private async Task<string[]> LoadFromFile(FileInfo file)
        {
            using (var readTask = File.ReadAllLinesAsync(file.Name))
                return await readTask;
        }

        private static bool IsUnnecessaryLog(string input)
        {
            return input[..3] == "C1 " || input[..3] == "C2 " || input[..3] == "C13";
        }

    }
}
