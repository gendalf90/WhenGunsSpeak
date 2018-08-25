using System.Net.Sockets;
using System.Threading.Tasks;

namespace Connection.Udp
{
    class MessageClient : IMessageClient
    {
        private readonly UdpClient udpClient;

        public MessageClient(UdpClient udpClient)
        {
            this.udpClient = udpClient;
        }

        public async Task<MessageDto> ReceiveAsync()
        {
            var data = await udpClient.ReceiveAsync();

            return new MessageDto
            {
                IP = data.RemoteEndPoint,
                Bytes = data.Buffer
            };
        }

        public async Task SendAsync(MessageDto data)
        {
            await udpClient.SendAsync(data.Bytes, data.Bytes.Length, data.IP);
        }

        public void Dispose()
        {
            udpClient.Close();
        }
    }
}
