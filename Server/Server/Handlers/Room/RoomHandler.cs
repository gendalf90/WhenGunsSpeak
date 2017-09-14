using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Server.Packets;
using Server.Rooms;
using System.IO;
using Newtonsoft.Json;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace Server.Handlers
{
    class RoomHandler : PacketHandler
    {
        private readonly IPacketSendStrategy sender;
        private readonly IRooms rooms;

        public RoomHandler(IPacketSendStrategy sender, IRooms rooms)
        {
            this.sender = sender;
            this.rooms = rooms;
        }

        public override async Task<bool> TryHandleAsync(Packet packet)
        {
            var header = ReadHeader(packet.Data);

            if(header.Action == "room")
            {
                await PostRoomAsync(header.Session, header.Description);
                return true;
            }

            if(header.Action == "rooms")
            {
                await SendAllRoomsToAsync(packet.IPEndPoint);
                return true;
            }

            return false;
        }

        private RoomHeader ReadHeader(byte[] data)
        {
            using (var receiveStream = new MemoryStream(data))
            using (var reader = new BinaryReader(receiveStream))
            {
                return JsonConvert.DeserializeObject<RoomHeader>(reader.ReadString());
            }
        }

        private async Task PostRoomAsync(string session, string description)
        {
            var newRoom = new Room(session, description);
            await rooms.AddAsync(newRoom);
        }

        private async Task SendAllRoomsToAsync(IPEndPoint iPEndPoint)
        {
            using (var responseStream = new MemoryStream())
            using (var writer = new BinaryWriter(responseStream))
            {
                var allRooms = await rooms.GetAllAsync();
                var jsonRooms = allRooms.Aggregate(new JObject(), (result, item) =>
                {
                    result.Add(item.Session, item.Description);
                    return result;
                });
                var jsonResult = new JObject
                {
                    ["Action"] = "rooms",
                    ["Rooms"] = jsonRooms
                };
                writer.Write(jsonResult.ToString());
                await sender.SendAsync(new Packet { IPEndPoint = iPEndPoint, Data = responseStream.ToArray() });
            }
        }
    }
}
