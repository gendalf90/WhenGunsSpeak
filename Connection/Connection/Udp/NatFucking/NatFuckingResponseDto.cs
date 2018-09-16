using MessagePack;
using System;

namespace Connection.Udp.NatFucking
{
    [MessagePackObject]
    class NatFuckingResponseDto
    {
        [Key(0)]
        public UdpMessageType MessageType { get; set; }

        [Key(1)]
        public Guid UserId { get; set; }

        [Key(2)]
        public byte[] Address { get; set; }

        [Key(3)]
        public int Port { get; set; }
    }
}
