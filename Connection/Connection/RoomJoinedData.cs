using System;

namespace Connection
{
    public sealed class RoomJoinedData
    {
        public Guid RoomId { get; set; }

        public byte[] SecurityKey { get; set; }
    }
}
