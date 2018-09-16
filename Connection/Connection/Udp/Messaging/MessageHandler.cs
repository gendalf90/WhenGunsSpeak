using Datagrammer;
using Datagrammer.MessagePack;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Connection.Udp.Messaging
{
    class MessageHandler : MessagePackHandler<MessageDto>
    {
        private readonly IObserver<MessageData> messageObserver;
        private readonly IOptions<UdpOptions> udpOptions;

        public MessageHandler(IObserver<MessageData> messageObserver, IOptions<UdpOptions> udpOptions)
        {
            this.messageObserver = messageObserver;
            this.udpOptions = udpOptions;
        }

        public override Task HandleAsync(IContext context, MessageDto data, IPEndPoint endPoint)
        {
            if(data.MessageType != UdpMessageType.Messaging)
            {
                return Task.CompletedTask;
            }

            if (data.UserId != udpOptions.Value.UserId)
            {
                return Task.CompletedTask;
            }

            messageObserver.OnNext(new MessageData
            {
                Bytes = data.Body,
                IP = endPoint
            });

            return Task.CompletedTask;
        }
    }
}
