using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Server.Packets
{
    class Packet
    {
        public IPEndPoint IPEndPoint { get; set; }

        public byte[] Data { get; set; }
    }
}
