using MessagePack;
using System;

namespace FuckNatService
{
    [MessagePackObject]
    public class RequestDto
    {
        [Key(0)]
        public Guid UserId { get; set; }
    }
}
