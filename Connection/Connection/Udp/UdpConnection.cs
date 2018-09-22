using Connection.Common;
using Connection.Udp.Messaging;
using Connection.Udp.NatFucking;
using Datagrammer;
using Datagrammer.MessagePack;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Connection.Udp
{
    class UdpConnection : IMessageConnection
    {
        private readonly IDatagramClient datagramClient;
        private readonly IOptions<MessageConnectionOptions> connectionOptions;
        private readonly IObserverComposite<MessageData> messageObserver;
        private readonly IObserverComposite<MyIPData> myIpObserver;

        private Timer natFuckingTimer;

        public UdpConnection(IDatagramClient datagramClient, 
                             IOptions<MessageConnectionOptions> connectionOptions,
                             IObserverComposite<MessageData> messageObserver,
                             IObserverComposite<MyIPData> myIpObserver)
        {
            this.datagramClient = datagramClient;
            this.connectionOptions = connectionOptions;
            this.messageObserver = messageObserver;
            this.myIpObserver = myIpObserver;
        }

        public async Task SendAsync(MessageData data)
        {
            await datagramClient.SendByMessagePackAsync(new MessageDto
            {
                Body = data.Bytes,
                MessageType = UdpMessageType.Messaging,
                UserId = connectionOptions.Value.UserId
            }, data.IP);
        }

        public void Start()
        {
            StartNatFuckingRequestSending();
        }

        private void StartNatFuckingRequestSending()
        {
            natFuckingTimer = new Timer(FuckNatAsync, null, connectionOptions.Value.NatFuckingPeriod, connectionOptions.Value.NatFuckingPeriod);
        }

        private async void FuckNatAsync(object state)
        {
            await datagramClient.SendByMessagePackAsync(new NatFuckingRequestDto
            {
                UserId = connectionOptions.Value.UserId
            }, connectionOptions.Value.NatFuckerAddress);
        }

        public IDisposable Subscribe(IObserver<MessageData> observer)
        {
            messageObserver.Add(observer);
            return new DisposeObserverCommand<MessageData>(messageObserver, observer);
        }

        public void Dispose()
        {
            natFuckingTimer.Dispose();
            datagramClient.Dispose();
        }

        public IDisposable Subscribe(IObserver<MyIPData> observer)
        {
            myIpObserver.Add(observer);
            return new DisposeObserverCommand<MyIPData>(myIpObserver, observer);
        }
    }
}
