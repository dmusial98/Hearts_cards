using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNetCore.Connections;
//using Microsoft.AspNetCore.SignalR.Client;

namespace SignalRConsole
{
    internal class Program
    {
        private Connection connection; 

        static void Main(string[] args)
        {
            
        }

        public void InitializeConnection()
        {
            connection = new Connection("http://localhost:5000");
        }

    }
}
