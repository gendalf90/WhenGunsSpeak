using System;
using System.Net;

namespace Connection
{
    public sealed class MessageConnectionOptions
    {
        public byte[] SecurityKey { get; set; }

        public Guid UserId { get; set; }

        public TimeSpan NatFuckingPeriod { get; set; }

        public IPEndPoint NatFuckerAddress { get; set; }

        public IPEndPoint ListeningPoint { get; set; }

        public int ReceivingParallelismDegree { get; set; }
    }
}
