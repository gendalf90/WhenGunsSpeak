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
        private StartMessagingConnectionHandler messagingConnectionHandler;
        private StartRoomConnectionHandler roomsConnectionHandler;

        public MessagesHandler()
        {
            receiveMessageUnsubscribers = new Dictionary<Guid, IDisposable>();
            messageConnections = new Dictionary<Guid, IMessageConnection>();
            sendingAddresses = new Dictionary<Guid, IPEndPoint>();
        }

        private void Awake()
        {
            synchronization = SynchronizationContext.Current;
            observable = FindObjectOfType<Observable>();
            messagingConnectionHandler = GetComponent<StartMessagingConnectionHandler>();
            roomsConnectionHandler = GetComponent<StartRoomConnectionHandler>();
        }

        private void OnEnable()
        {
            roomsConnectionHandler.OnConnected += ConnectionHandler_OnConnected;
            messagingConnectionHandler.OnStarted += MessagingConnectionHandler_OnStarted;
        }

        private void MessagingConnectionHandler_OnStarted(object sender, StartMessagingConnectionEventArgs e)
        {
            sendingAddresses.Add(e.UserId, null);
            receiveMessageUnsubscribers.Add(e.UserId, e.MessageConnection.Subscribe<MessageData>(next: MessageDataHandle));
            messageConnections.Add(e.UserId, e.MessageConnection);
        }

        private void ConnectionHandler_OnConnected(object sender, StartRoomConnectionEventArgs e)
        {
            e.RoomConnection.Subscribe<UserIPData>(next: UserIPDataHandler);
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

        private void MessageDataHandle(MessageData data)
        {
            synchronization.Post(state =>
            {
                observable.Publish(new MessageIsReceivedEvent(data.Bytes));
            }, data);
        }

        //public void RemoveConnection(Guid userId)
        //{
        //    if(!sendingAddresses.Remove(userId))
        //    {
        //        return;
        //    }

        //    var receiveMessageUnsubscriber = receiveMessageUnsubscribers[userId];
        //    receiveMessageUnsubscriber.Dispose();
        //    receiveMessageUnsubscribers.Remove(userId);
        //    messageConnections.Remove(userId);
        //}
    }
}
