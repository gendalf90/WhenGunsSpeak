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

        public MyIPHandler()
        {
            unsubscribers = new Dictionary<Guid, IDisposable>();
        }

        public void Start(IRoomConnection connection)
        {
            roomConnection = connection;
        }

        public void AddConnection(IMessageConnection connection, Guid userId)
        {
            var unsubscriber = connection.Subscribe<MyIPData>(next: async value =>
            {
                await roomConnection.TellMyIpAsync(userId, value.IP.Address, value.IP.Port);
            });
            unsubscribers.Add(userId, unsubscriber);
        }

        public void RemoveConnection(Guid userId)
        {
            IDisposable unsubscriber = null;

            if (!unsubscribers.TryGetValue(userId, out unsubscriber))
            {
                return;
            }

            unsubscriber.Dispose();
            unsubscribers.Remove(userId);
        }
    }
}
