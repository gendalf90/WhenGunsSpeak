using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Sockets;

namespace FuckNatService.MessageClient
{
    class MessageClientCreator : IMessageClientCreator
    {
        private readonly IOptions<UdpOptions> options;

        public MessageClientCreator(IOptions<UdpOptions> options)
        {
            this.options = options;
        }

        public IMessageClient Create()
        {
            var ipAddress = IPAddress.Any;
            var port = options.Value.Port;
            var listenOn = new IPEndPoint(ipAddress, port);
            var udpClient = new UdpClient(listenOn);
            return new UdpMessageClient(udpClient);
        }
    }
}
