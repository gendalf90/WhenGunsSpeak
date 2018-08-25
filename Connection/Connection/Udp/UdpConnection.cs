using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Connection.Udp
{
    class UdpConnection : IHostedService, IUdpConnection
    {
        private readonly IOptions<UdpOptions> udpOptions;
        private readonly IMessageCreator messageCreator;
        private readonly IMessageClientCreator messageClientCreator;

        private IMessageClient messageClient;
        private Timer natFuckingTimer;
        private Guid natFuckingSessionId;

        public UdpConnection()
        {

        }


        public async Task SendAsync(MessageData data)
        {
            var message = messageCreator.Create();
            await message.SendAsync(messageClient, data);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            InitializeClient();
            InitializeNatFuckingSession();
            StartNatFucking();
            return Task.CompletedTask;
        }

        private void InitializeNatFuckingSession()
        {
            natFuckingSessionId = Guid.NewGuid();
        }

        private void InitializeClient()
        {
            messageClient = messageClientCreator.Create();
        }

        private void StartNatFucking()
        {
            natFuckingTimer = new Timer(FuckNatAsync, null, udpOptions.Value.NatFuckingPeriod, udpOptions.Value.NatFuckingPeriod);
        }

        private async void FuckNatAsync(object sessionId)
        {
            var message = messageCreator.Create();
            await message.SendFuckNatRequestAsync(messageClient);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            natFuckingTimer.Dispose();
            messageClient.Dispose();
            return Task.CompletedTask;
        }

        
    }
}
