using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Server.Packets;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Server.Handlers
{
    class SessionHandler : PacketHandler
    {
        private readonly IPacketSendStrategy sender;

        public SessionHandler(IPacketSendStrategy sender)
        {
            this.sender = sender;
        }

        public override async Task<bool> TryHandleAsync(Packet packet)
        {
            using (var receiveStream = new MemoryStream(packet.Data))
            using (var reader = new BinaryReader(receiveStream))
            using (var responseStream = new MemoryStream())
            using (var writer = new BinaryWriter(responseStream))
            {
                var header = JsonConvert.DeserializeObject<SessionHeader>(reader.ReadString());

                if (header.Action != "session")
                {
                    return false;
                }

                var response = new { Action = "session", Session = new Session(packet.IPEndPoint).ToString() };
                var json = JsonConvert.SerializeObject(response);
                writer.Write(json);
                await sender.SendAsync(new Packet { IPEndPoint = packet.IPEndPoint, Data = responseStream.ToArray() });
                return true;
            }
        }
    }
}
