using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.Packets
{
    class Udp : IPacketReceiveStrategy, IPacketSendStrategy
    {
        private UdpClient client;

        public Udp(IPEndPoint listenEndPoint)
        {
            client = new UdpClient(listenEndPoint);
        }

        public async Task SendAsync(Packet packet)
        {
            await client.SendAsync(packet.Data, packet.Data.Length, packet.IPEndPoint);
        }

        public async Task<Packet> ReceiveAsync()
        {
            var result = await client.ReceiveAsync();
            return new Packet { IPEndPoint = result.RemoteEndPoint, Data = result.Buffer };
        }
    }
}
