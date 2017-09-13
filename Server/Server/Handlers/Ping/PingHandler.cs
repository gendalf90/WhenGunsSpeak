using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Server.Packets;
using System.IO;
using Newtonsoft.Json;
using System.Linq;

namespace Server.Handlers
{
    class PingHandler : PacketHandler
    {
        private readonly IPacketSendStrategy sender;

        public PingHandler(IPacketSendStrategy sender)
        {
            this.sender = sender;
        }

        public override async Task<bool> TryHandleAsync(Packet packet)
        {
            using (var receiveStream = new MemoryStream(packet.Data))
            using (var reader = new BinaryReader(receiveStream))
            {
                var header = JsonConvert.DeserializeObject<PingHeader>(reader.ReadString());

                if (header.Action != "ping")
                {
                    return false;
                }

                var toSession = new Session(header.To);
                var toEndPoint = toSession.GetIPEndPoint();
                var toPacket = new Packet { IPEndPoint = toEndPoint, Data = packet.Data };
                await sender.SendAsync(toPacket);
                return true;
            }
        }
    }
}
