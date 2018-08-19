using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
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
        private readonly ILogger<GetYourIPService> logger;

        private UdpClient udpClient;

        public GetYourIPService(IOptions<UdpOptions> options, ILogger<GetYourIPService> logger)
        {
            this.options = options;
            this.logger = logger;
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
                await GetYourIpSafeAsync();
            };
        }

        private void InitializeClient()
        {
            var ipAddress = IPAddress.Any;
            var port = options.Value.Port;
            var listenOn = new IPEndPoint(ipAddress, port);
            udpClient = new UdpClient(listenOn);
        }

        private async Task GetYourIpSafeAsync()
        {
            try
            {
                await GetYourIpAsync();
            }
            catch(Exception e)
            {
                logger.LogError(e, "Error while ip getting");
            }
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
