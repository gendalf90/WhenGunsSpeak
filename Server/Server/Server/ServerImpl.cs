using Server.Handlers;
using Server.Packets;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class ServerImpl : IServer
    {
        private readonly IPacketHandler handler;
        private readonly IPacketReceiveStrategy receiver;

        public ServerImpl(IPacketHandler handler, IPacketReceiveStrategy receiver)
        {
            this.handler = handler;
            this.receiver = receiver;
        }

        public void Run()
        {
            ListenAsync();
        }

        private async void ListenAsync()
        {
            var packet = await receiver.ReceiveAsync();
            ListenAsync();
            await handler.HandleAsync(packet);
        }
    }
}
