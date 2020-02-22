using Sylver.ClientAwaiter.Common;
using Sylver.Network.Client;
using Sylver.Network.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sylver.ClientAwaiter
{
    public class TcpClient : NetClient
    {
        private readonly IDictionary<Guid, TaskCompletionSource<INetPacketStream>> _pendingTasks;

        public TcpClient()
            : base(new NetClientConfiguration("127.0.0.1", 4444))
        {
            _pendingTasks = new Dictionary<Guid, TaskCompletionSource<INetPacketStream>>();
        }

        protected override void OnConnected()
        {
            Console.WriteLine($"Connected to {ClientConfiguration.Host}:{ClientConfiguration.Port}");
        }

        public override void HandleMessage(INetPacketStream packet)
        {
            var header = (PacketHeaderType)packet.Read<int>();
            Guid uniqueTaskId = Guid.Parse(packet.Read<string>());

            if (_pendingTasks.TryGetValue(uniqueTaskId, out TaskCompletionSource<INetPacketStream> taskCompletionSource))
            {
                taskCompletionSource.SetResult(packet);
            }
            else
            {
                // Handle other "non-awaited" packets here

                switch (header)
                {
                    default:
                        Console.WriteLine($"Unknown packet");
                        break;
                }
            }
        }

        public Task<INetPacketStream> QueryDataAsync(string data)
        {
            var taskUniqueId = Guid.NewGuid();
            var taskCompletionSource = new TaskCompletionSource<INetPacketStream>();
            using var packet = new NetPacket();

            packet.Write((int)PacketHeaderType.QueryData);
            packet.Write(taskUniqueId.ToString());
            packet.Write(data);

            Send(packet);

            _pendingTasks.Add(taskUniqueId, taskCompletionSource);

            return taskCompletionSource.Task;
        }
    }
}
