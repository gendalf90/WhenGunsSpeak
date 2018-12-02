using Connection;
using Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using UnityEngine;
using Utils;

namespace Server
{
    class MessagesHandler : MonoBehaviour
    {
        private Observable observable;
        private SynchronizationContext synchronization;
        private IDictionary<Guid, IDisposable> receiveMessageUnsubscribers;
        private IDictionary<Guid, IMessageConnection> messageConnections;
        private IDictionary<Guid, IPEndPoint> sendingAddresses;

        public MessagesHandler()
        {
            receiveMessageUnsubscribers = new Dictionary<Guid, IDisposable>();
            messageConnections = new Dictionary<Guid, IMessageConnection>();
            sendingAddresses = new Dictionary<Guid, IPEndPoint>();
        }

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
        }

        private void Start()
        {
            synchronization = SynchronizationContext.Current;
        }

        public void Start(IRoomConnection connection)
        {
            connection.Subscribe<UserIPData>(next: UserIPDataHandler);
            observable.Subscribe<SendMessageCommand>(SendMessageHandle);
        }

        private void UserIPDataHandler(UserIPData data)
        {
            synchronization.Post(state =>
            {
                if(sendingAddresses.ContainsKey(data.UserId))
                {
                    sendingAddresses[data.UserId] = data.IP;
                }
            }, data);
        }

        private async void SendMessageHandle(SendMessageCommand command)
        {
            var sendingInfo = messageConnections.Where(pair => sendingAddresses[pair.Key] != null)
                                                .Select(pair => new { Connection = pair.Value, Address = sendingAddresses[pair.Key] })
                                                .ToList();

            foreach(var info in sendingInfo)
            {
                await info.Connection.SendAsync(new MessageData
                {
                    Bytes = command.Data,
                    IP = info.Address
                });
            }
        }

        public void AddConnection(IMessageConnection connection, Guid userId)
        {
            sendingAddresses.Add(userId, null);
            receiveMessageUnsubscribers.Add(userId, connection.Subscribe<MessageData>(next: MessageDataHandle));
            messageConnections.Add(userId, connection);
        }

        private void MessageDataHandle(MessageData data)
        {
            synchronization.Post(state =>
            {
                observable.Publish(new MessageIsReceivedEvent(data.Bytes));
            }, data);
        }

        public void RemoveConnection(Guid userId)
        {
            if(!sendingAddresses.Remove(userId))
            {
                return;
            }

            var receiveMessageUnsubscriber = receiveMessageUnsubscribers[userId];
            receiveMessageUnsubscriber.Dispose();
            receiveMessageUnsubscribers.Remove(userId);
            messageConnections.Remove(userId);
        }
    }
}
