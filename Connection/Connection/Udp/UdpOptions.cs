﻿using System;
using System.Net;

namespace Connection.Udp
{
    class UdpOptions
    {
        public TimeSpan NatFuckingPeriod { get; set; }

        public IPEndPoint NatFuckerAddress { get; set; }

        public IPEndPoint MessagingEndPoint { get; set; }

        public Guid UserId { get; set; }
    }
}
