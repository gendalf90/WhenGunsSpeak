using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FuckNatService
{
    class GetYourIPService : IHostedService
    {
        private readonly IOptions<UdpOptions> options;

        private UdpClient udpClient;

        public GetYourIPService(IOptions<UdpOptions> options)
        {
            this.options = options;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            ProcessAsync();
            return Task.CompletedTask;
        }

        private async void ProcessAsync()
        {
            InitializeClient();

            while (true)
            {
                await GetYourIpAsync();
            };
        }

        private void InitializeClient()
        {
            var ipAddress = IPAddress.Any;
            var port = options.Value.Port;
            var listenOn = new IPEndPoint(ipAddress, port);
            udpClient = new UdpClient(listenOn);
        }

        private async Task GetYourIpAsync()
        {
            var request = await udpClient.ReceiveAsync();
            var endPointString = request.RemoteEndPoint.ToString();
            var endPointBytes = Encoding.UTF8.GetBytes(endPointString);
            await udpClient.SendAsync(endPointBytes, endPointBytes.Length, request.RemoteEndPoint);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            udpClient.Close();
            return Task.CompletedTask;
        }
    }
}
