using Hearts_server.ResultsWriterReader;
using HeartsServer.GameLogic.History;

namespace HeartsServer.ResultsWriterReader
{
    public abstract class GameFileReader : IGameReader
    {
        public string fileName;

        protected GameFileReader(string fileName) => this.fileName = fileName;
        protected GameFileReader() 
        { 
            this.fileName = string.Empty; 
        }

        protected FileInfo GetFile()
        {
            DirectoryInfo directory = new DirectoryInfo("LogFiles");
            FileInfo file = null;
            if (fileName == string.Empty)
                file = directory.GetFiles().OrderByDescending(f => f.LastWriteTime).First();
            else
                file = directory.GetFiles().FirstOrDefault(f => f.Name == fileName);

            if (file == null)
                throw new FileNotFoundException();

            return file;
        }

        public virtual Task<GameHistory> GetGameHistoryAsync()
        {
            return Task.FromResult(new GameHistory());
        }
    }
}
