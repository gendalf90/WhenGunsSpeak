using System;
using System.Net;

namespace Connection
{
    public sealed class ConnectionOptions
    {
        public Guid UserId { get; set; }

        public byte[] SecurityKey { get; set; }

        public TimeSpan NatFuckingPeriod { get; set; }

        public IPEndPoint NatFuckerAddress { get; set; }

        public IPEndPoint MessagingEndPoint { get; set; }

        public Uri RoomsAddress { get; set; }
    }
}
