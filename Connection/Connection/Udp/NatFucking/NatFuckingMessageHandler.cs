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
        private readonly IOptions<UdpOptions> connectionOptions;

        public NatFuckingMessageHandler(IObserver<MyIPData> myIPObserver, IOptions<UdpOptions> connectionOptions)
        {
            this.myIPObserver = myIPObserver;
            this.connectionOptions = connectionOptions;
        }

        public override Task HandleAsync(IContext context, NatFuckingResponseDto data, IPEndPoint endPoint)
        {
            if(data.MessageType != UdpMessageType.NatFucking)
            {
                return Task.CompletedTask;
            }

            if(data.UserId != connectionOptions.Value.UserId)
            {
                return Task.CompletedTask;
            }

            myIPObserver.OnNext(new MyIPData
            {
                UserId = connectionOptions.Value.UserId,
                IP = new IPEndPoint(new IPAddress(data.Address), data.Port)
            });

            return Task.CompletedTask;
        }
    }
}
