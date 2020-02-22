using System;

namespace Sylver.TCPServer
{
    class Program
    {
        static void Main(string[] args)
        {
            using var server = new TcpServer();

            server.Start();

            Console.ReadKey();
        }
    }
}
