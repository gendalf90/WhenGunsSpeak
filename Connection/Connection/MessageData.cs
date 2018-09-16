using System.Net;

namespace Connection
{
    public sealed class MessageData
    {
        public IPEndPoint IP { get; set; }

        public byte[] Bytes { get; set; }
    }
}
