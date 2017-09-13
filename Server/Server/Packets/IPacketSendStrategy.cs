using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Server.Packets
{
    interface IPacketSendStrategy
    {
        Task SendAsync(Packet packet);
    }
}
