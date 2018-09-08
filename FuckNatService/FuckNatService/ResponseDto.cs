using MessagePack;
using System;

namespace FuckNatService
{
    [MessagePackObject(keyAsPropertyName: true)]
    public class ResponseDto
    {
        public Guid SessionID { get; set; }

        public byte[] Address { get; set; }

        public int Port { get; set; }
    }
}
