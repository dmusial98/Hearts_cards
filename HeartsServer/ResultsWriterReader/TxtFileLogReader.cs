using Hearts_server.ResultsWriterReader;
using HeartsServer.GameLogic.History;

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


            return new GameHistory();
        }

        private async Task<string[]> LoadFromFile(FileInfo file)
        {
            using (var readTask = File.ReadAllLinesAsync(file.Name))
                return await readTask;
        }
    }
}
