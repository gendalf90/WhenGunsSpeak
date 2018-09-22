using System;
using System.Net;

namespace Connection.Udp
{
    internal class UdpOptions
    {
        public byte[] SecurityKey { get; set; }

        public Guid UserId { get; set; }

        public TimeSpan NatFuckingPeriod { get; set; }

        public IPEndPoint NatFuckerAddress { get; set; }
    }
}
