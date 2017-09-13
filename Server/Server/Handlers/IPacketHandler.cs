using Server.Packets;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Server.Handlers
{
    interface IPacketHandler
    {
        Task HandleAsync(Packet packet);
    }
}
