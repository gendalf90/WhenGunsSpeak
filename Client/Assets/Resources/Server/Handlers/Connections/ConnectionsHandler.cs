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

        private PingHandler pingHandler;

        private List<Connection> connections;

        private Guid currentGuid;
        private Connection currentConnection;

        private void Awake()
        {
            pingHandler = GetComponent<PingHandler>();
        }

        private void Start()
        {
            connections = new List<Connection>();
            pingHandler.OnPingReceive += PingReceive;
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
                OnDisconnect.SafeRaise(this, new ConnectionEventArgs(connection.Guid));
            }
        }

        private void PingReceive(object sender, ReceivePingEventArgs args)
        {
            currentGuid = args.From;

            TryFindConnection();
            TryUpdateConnection();
            TryAddConnection();
        }

        public event EventHandler<ConnectionEventArgs> OnConnect;

        public event EventHandler<ConnectionEventArgs> OnDisconnect;

        private void TryFindConnection()
        {
            currentConnection = connections.Find(connection => connection.Guid == currentGuid);
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

            var newConnection = Connection.StartConnection(currentGuid, connectionTimeout);
            connections.Add(newConnection);
            OnConnect.SafeRaise(this, new ConnectionEventArgs(newConnection.Guid));
        }

        private void OnDestroy()
        {
            pingHandler.OnPingReceive -= PingReceive;
        }
    }
}
