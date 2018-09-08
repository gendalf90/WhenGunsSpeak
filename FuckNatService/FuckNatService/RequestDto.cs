using MessagePack;
using System;

namespace FuckNatService
{
    [MessagePackObject(keyAsPropertyName: true)]
    public class RequestDto
    {
        public Guid SessionID { get; set; }
    }
}
