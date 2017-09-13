using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Server.Packets;
using System.IO;
using Newtonsoft.Json.Linq;
using Server.Clients;
using Newtonsoft.Json;
using System.Linq;

namespace Server.Handlers
{
    class SendHandler : PacketHandler
    {
        private readonly IPacketSendStrategy sender;

        public SendHandler(IPacketSendStrategy sender)
        {
            this.sender = sender;
        }

        public override async Task<bool> TryHandleAsync(Packet packet)
        {
            using (var receiveStream = new MemoryStream(packet.Data))
            using (var reader = new BinaryReader(receiveStream))
            {
                var header = JsonConvert.DeserializeObject<SendHeader>(reader.ReadString());

                if (header.Action != "send")
                {
                    return false;
                }

                var sendingPackets = header.To.Select(data => new Session(data))
                                              .Select(session => session.GetIPEndPoint())
                                              .Select(endPoint => new Packet { IPEndPoint = endPoint, Data = packet.Data });

                foreach(var sendingPacket in sendingPackets)
                {
                    await sender.SendAsync(packet);
                }

                return true;
            }
        }
    }
}
