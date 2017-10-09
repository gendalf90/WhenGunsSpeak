using Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Server
{
    class ConnectionsHandler : MonoBehaviour
    {
        [SerializeField]
        private float connectionTimeout;

        private List<Connection> connections;

        private string currentSession;
        private Connection currentConnection;

        private Observable observable;

        public ConnectionsHandler()
        {
            connections = new List<Connection>();
        }

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
        }

        private void OnEnable()
        {
            observable.Subscribe<OnPingEvent>(Receive);
        }

        private void OnDisable()
        {
            observable.Unsubscribe<OnPingEvent>(Receive);
        }

        private void Update()
        {
            RemoveTimeoutConnections();
        }

        private void RemoveTimeoutConnections()
        {
            var timeoutConnections = connections.Where(connection => connection.IsTimeout).ToList();

            foreach (var connection in timeoutConnections)
            {
                connections.Remove(connection);
                observable.Publish(new OnDisconnectionEvent(connection.Session));
            }
        }

        private void Receive(OnPingEvent e)
        {
            currentSession = e.FromSession;

            TryFindConnection();
            TryUpdateConnection();
            TryAddConnection();
        }

        private void TryFindConnection()
        {
            currentConnection = connections.Find(connection => connection.Session == currentSession);
        }

        private void TryUpdateConnection()
        {
            if (currentConnection != null)
            {
                currentConnection.Ping();
            }
        }

        private void TryAddConnection()
        {
            if (currentConnection != null)
            {
                return;
            }

            var newConnection = Connection.StartConnection(currentSession, connectionTimeout);
            connections.Add(newConnection);
            observable.Publish(new OnConnectionEvent(newConnection.Session));
        }
    }
}
