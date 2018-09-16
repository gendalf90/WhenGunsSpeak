using Datagrammer;
using Datagrammer.MessagePack;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Connection.Udp.NatFucking
{
    class NatFuckingMessageHandler : MessagePackHandler<NatFuckingResponseDto>
    {
        private readonly IObserver<MyIPData> myIPObserver;
        private readonly IOptions<UdpOptions> udpOptions;

        public NatFuckingMessageHandler(IObserver<MyIPData> myIPObserver, IOptions<UdpOptions> udpOptions)
        {
            this.myIPObserver = myIPObserver;
            this.udpOptions = udpOptions;
        }

        public override Task HandleAsync(IContext context, NatFuckingResponseDto data, IPEndPoint endPoint)
        {
            if(data.MessageType != UdpMessageType.NatFucking)
            {
                return Task.CompletedTask;
            }

            if(data.UserId != udpOptions.Value.UserId)
            {
                return Task.CompletedTask;
            }

            myIPObserver.OnNext(new MyIPData
            {
                IP = new IPEndPoint(new IPAddress(data.Address), data.Port)
            });

            return Task.CompletedTask;
        }
    }
}
