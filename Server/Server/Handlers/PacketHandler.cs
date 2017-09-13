using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Server.Packets;

namespace Server.Handlers
{
    abstract class PacketHandler : IPacketHandler
    {
        private IPacketHandler successor;

        public void SetSuccessor(IPacketHandler successor)
        {
            this.successor = successor;
        }

        public async Task HandleAsync(Packet packet)
        {
            var handleSuccess = await TryHandleAsync(packet);

            if(!handleSuccess && successor != null)
            {
                await successor.HandleAsync(packet);
            }
        }

        public abstract Task<bool> TryHandleAsync(Packet packet);
    }
}
