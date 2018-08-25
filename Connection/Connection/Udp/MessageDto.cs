using System.Net;

namespace Connection.Udp
{
    public class MessageDto
    {
        public IPEndPoint IP { get; set; }

        public byte[] Bytes { get; set; }
    }
}