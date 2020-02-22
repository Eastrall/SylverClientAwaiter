using Sylver.ClientAwaiter.Common;
using Sylver.Network.Data;
using Sylver.Network.Server;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Sylver.TCPServer
{
    public class Client : NetServerClient
    {
        public Client(Socket socketConnection) 
            : base(socketConnection)
        {
        }

        public override void HandleMessage(INetPacketStream packet)
        {
            var header = (PacketHeaderType)packet.Read<int>();

            switch (header)
            {
                case PacketHeaderType.QueryData:
                    string uniqueId = packet.Read<string>();
                    string data = packet.Read<string>();

                    Task.Run(async () => await QueryData(uniqueId, data));
                    break;
            }
        }

        private async Task QueryData(string uniqueId, string data)
        {
            // Simulate hard work
            await Task.Delay(5000);

            // Send data back to client
            using var packet = new NetPacket();

            packet.Write((int)PacketHeaderType.ResponseData);
            packet.Write(uniqueId);
            packet.Write($"Your data: {data}");

            Send(packet);
        }
    }
}
