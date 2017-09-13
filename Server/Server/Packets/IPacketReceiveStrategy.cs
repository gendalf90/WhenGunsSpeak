using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Server.Packets
{
    interface IPacketReceiveStrategy
    {
        Task<Packet> ReceiveAsync();
    }
}
