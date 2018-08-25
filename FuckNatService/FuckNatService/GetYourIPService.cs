using FuckNatService.MessageClient;
using FuckNatService.Messages;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FuckNatService
{
    class GetYourIPService : IHostedService
    {
        private readonly IMessageClientCreator messageClientCreator;
        private readonly IRequestCreator requestCreator;
        private readonly ILogger<GetYourIPService> logger;

        private IMessageClient messageClient;

        public GetYourIPService(IMessageClientCreator messageClientCreator,
                                IRequestCreator requestCreator, 
                                ILogger<GetYourIPService> logger)
        {
            this.messageClientCreator = messageClientCreator;
            this.requestCreator = requestCreator;
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
            messageClient = messageClientCreator.Create();
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
            var request = requestCreator.Create();
            await request.LoadAsync(messageClient);
            await request.SendResponseIfValidAsync(messageClient);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            messageClient.Dispose();
            return Task.CompletedTask;
        }
    }
}
