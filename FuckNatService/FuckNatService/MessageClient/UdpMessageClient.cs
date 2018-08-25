using System.Net.Sockets;
using System.Threading.Tasks;

namespace FuckNatService.MessageClient
{
    class UdpMessageClient : IMessageClient
    {
        private readonly UdpClient udpClient;

        public UdpMessageClient(UdpClient udpClient)
        {
            this.udpClient = udpClient;
        }

        public async Task<MessageDto> ReceiveAsync()
        {
            var data = await udpClient.ReceiveAsync();

            return new MessageDto
            {
                EndPoint = data.RemoteEndPoint,
                Bytes = data.Buffer
            };
        }

        public async Task SendAsync(MessageDto data)
        {
            await udpClient.SendAsync(data.Bytes, data.Bytes.Length, data.EndPoint);
        }

        public void Dispose()
        {
            udpClient.Close();
        }
    }
}
