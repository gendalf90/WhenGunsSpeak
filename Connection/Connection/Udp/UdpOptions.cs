using System;
using System.Net;

namespace Connection.Udp
{
    class UdpOptions
    {
        public TimeSpan NatFuckingPeriod { get; set; }

        public int SendBufferSize { get; set; }

        public int ReceiveBufferSize { get; set; }

        public IPEndPoint NatFuckerAddress { get; set; }
    }
}
