using System.Net;

namespace FuckNatService.MessageClient
{
    class MessageDto
    {
        public IPEndPoint EndPoint { get; set; }

        public byte[] Bytes { get; set; }
    }
}
