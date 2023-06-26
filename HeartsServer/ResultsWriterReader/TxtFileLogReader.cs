using Hearts_server.ResultsWriterReader;
using HeartsServer.GameLogic.History;

namespace HeartsServer.ResultsWriterReader
{
    public class TxtFileLogReader : IGameReader
    {
        string _fileName = string.Empty;

        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        public TxtFileLogReader(string fileName) => _fileName = fileName;
        public TxtFileLogReader() { }

        public async Task<GameHistory> GetGameHistoryFromFileAsync()
        {
            DirectoryInfo directory = new DirectoryInfo("LogFiles");
            FileInfo file = null;
            if (_fileName == string.Empty)
                file = directory.GetFiles().OrderByDescending(f => f.LastWriteTime).First();
            else
                file = directory.GetFiles().FirstOrDefault(f => f.Name == _fileName);

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
