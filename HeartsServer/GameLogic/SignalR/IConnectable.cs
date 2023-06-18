namespace Hearts_server.GameLogic.SignalR
{
    public interface IConnectable
    {

        public void ReceivedSignalFromClient();

        public void SendSignalToClient();
    }
}
