using Sylver.Network.Data;
using System;
using System.Threading.Tasks;

namespace Sylver.ClientAwaiter
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var client = new TcpClient();

            client.Connect();

            while (true)
            {
                string data = Console.ReadLine();

                INetPacketStream packet = await client.QueryDataAsync(data);

                string serverData = packet.Read<string>();

                Console.WriteLine($"Data from server: {serverData}");
            }
        }
    }
}
