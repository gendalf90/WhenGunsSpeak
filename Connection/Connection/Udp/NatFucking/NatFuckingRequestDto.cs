using MessagePack;
using System;

namespace Connection.Udp.NatFucking
{
    [MessagePackObject]
    class NatFuckingRequestDto
    {
        [Key(0)]
        public Guid UserId { get; set; }
    }
}
