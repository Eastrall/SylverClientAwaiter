using Sylver.Network.Server;
using System;

namespace Sylver.TCPServer
{
    public class TcpServer : NetServer<Client>
    {
        public TcpServer()
            : base(new NetServerConfiguration("127.0.0.1", 4444))
        {
        }

        protected override void OnAfterStart()
        {
            Console.WriteLine($"Server listening on {ServerConfiguration.Host}:{ServerConfiguration.Port}");
        }
    }
}
