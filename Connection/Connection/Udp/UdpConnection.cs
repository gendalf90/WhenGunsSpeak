using Connection.Udp.Messaging;
using Connection.Udp.NatFucking;
using Datagrammer;
using Datagrammer.MessagePack;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;

namespace Connection.Udp
{
    class UdpConnection : IUdpConnection
    {
        private readonly IDatagramSender datagramSender;
        private readonly IOptions<UdpOptions> udpOptions;

        private Timer natFuckingTimer;

        public UdpConnection(IDatagramSender datagramSender, IOptions<UdpOptions> udpOptions)
        {
            this.datagramSender = datagramSender;
            this.udpOptions = udpOptions;
        }

        public async Task SendAsync(MessageData data)
        {
            await datagramSender.SendByMessagePackAsync(new MessageDto
            {
                Body = data.Bytes,
                MessageType = UdpMessageType.Messaging,
                UserId = udpOptions.Value.UserId
            }, data.IP);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            StartNatFuckingRequestSending();
            return Task.CompletedTask;
        }

        private void StartNatFuckingRequestSending()
        {
            natFuckingTimer = new Timer(FuckNatAsync, null, udpOptions.Value.NatFuckingPeriod, udpOptions.Value.NatFuckingPeriod);
        }

        private async void FuckNatAsync(object state)
        {
            await datagramSender.SendByMessagePackAsync(new NatFuckingRequestDto
            {
                UserId = udpOptions.Value.UserId
            }, udpOptions.Value.NatFuckerAddress);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            natFuckingTimer.Dispose();
            return Task.CompletedTask;
        }
    }
}
