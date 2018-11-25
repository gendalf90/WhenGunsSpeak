using Connection;
using MessagePack;
using Messages;
using System;
using System.Net;
using System.Threading;
using UnityEngine;

namespace Server
{
    class MessagesHandler : MonoBehaviour, IObserver<MessageData>
    {
        private Observable observable;
        private SynchronizationContext synchronization;
        private IMessageConnection messageConnection;
        private IDisposable unsubscriber;
        private IPEndPoint toSendAddress;

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
        }

        private void Start()
        {
            synchronization = SynchronizationContext.Current;
        }

        private void OnEnable()
        {
            unsubscriber = messageConnection.Subscribe(this);
            observable.Subscribe<SendMessageCommand>(SendMessageHandler);
        }

        private void OnDisable()
        {
            observable.Unsubscribe<SendMessageCommand>(SendMessageHandler);
            unsubscriber.Dispose();
        }

        private async void SendMessageHandler(SendMessageCommand command)
        {
            await messageConnection.SendAsync(new MessageData
            {
                Bytes = MessagePackSerializer.FromJson(command.Json),
                IP = toSendAddress
            });
        }

        public void Start(IMessageConnection messageConnection, IPEndPoint toSendAddress)
        {
            this.messageConnection = messageConnection;
            this.toSendAddress = toSendAddress;
            enabled = true;
        }

        void IObserver<MessageData>.OnCompleted()
        {
        }

        void IObserver<MessageData>.OnError(Exception error)
        {
        }

        void IObserver<MessageData>.OnNext(MessageData value)
        {
            var jsonData = MessagePackSerializer.ToJson(value.Bytes);

            synchronization.Post(state =>
            {
                observable.Publish(new MessageIsReceivedEvent(jsonData));
            }, value);
        }
    }
}
