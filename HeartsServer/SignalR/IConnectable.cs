namespace HeartsServer.SignalR
{
    public interface IConnectable
    {

        public void ReceivedSignalFromClient();

        public void SendSignalToClient();
    }
}
