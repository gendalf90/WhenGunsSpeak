using Connection;
using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Server
{
    class MyIPHandler : MonoBehaviour
    {
        private IRoomConnection roomConnection;
        private IDictionary<Guid, IDisposable> unsubscribers;
        private StartRoomConnectionHandler roomsConnectionHandler;
        private StartMessagingConnectionHandler messagingConnectionHandler;

        public MyIPHandler()
        {
            unsubscribers = new Dictionary<Guid, IDisposable>();
        }

        private void Awake()
        {
            roomsConnectionHandler = GetComponent<StartRoomConnectionHandler>();
            messagingConnectionHandler = GetComponent<StartMessagingConnectionHandler>();
        }

        private void OnEnable()
        {
            roomsConnectionHandler.OnConnected += ConnectionHandler_OnConnected;
            messagingConnectionHandler.OnStarted += MessagingConnectionHandler_OnStarted;
        }

        private void MessagingConnectionHandler_OnStarted(object sender, StartMessagingConnectionEventArgs e)
        {
            var unsubscriber = e.MessageConnection.Subscribe<MyIPData>(next: async value =>
            {
                await roomConnection.TellMyIpAsync(e.UserId, value.IP.Address, value.IP.Port);
            });
            unsubscribers.Add(e.UserId, unsubscriber);
        }

        private void ConnectionHandler_OnConnected(object sender, StartRoomConnectionEventArgs e)
        {
            roomConnection = e.RoomConnection;
        }

        //public void RemoveConnection(Guid userId)
        //{
        //    IDisposable unsubscriber = null;

        //    if (!unsubscribers.TryGetValue(userId, out unsubscriber))
        //    {
        //        return;
        //    }

        //    unsubscriber.Dispose();
        //    unsubscribers.Remove(userId);
        //}
    }
}
