using MessagePack;
using System;

namespace Connection.Udp.Messaging
{
    [MessagePackObject]
    class MessageDto
    {
        [Key(0)]
        public UdpMessageType MessageType { get; set; }

        [Key(1)]
        public Guid UserId { get; set; }

        [Key(2)]
        public byte[] Body { get; set; }
    }
}
