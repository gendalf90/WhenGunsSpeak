using MessagePack;
using System;

namespace FuckNatService
{
    [MessagePackObject]
    public class ResponseDto
    {
        [Key(0)]
        public byte MessageType { get; set; }

        [Key(1)]
        public Guid UserId { get; set; }

        [Key(2)]
        public byte[] Address { get; set; }

        [Key(3)]
        public int Port { get; set; }
    }
}
