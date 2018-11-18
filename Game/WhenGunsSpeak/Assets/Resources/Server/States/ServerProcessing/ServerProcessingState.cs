using Messages;
using System.Collections.Generic;
using UnityEngine;
using Connection;

namespace Server
{
    class ServerProcessingState : MonoBehaviour
    {
        [SerializeField]
        private int maxConnections;

        private Observable observable;
        private IRoomConnection roomConnection;

        private HashSet<string> clients;

        public ServerProcessingState()
        {
            clients = new HashSet<string>();
        }

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
        }

        public void SetRoomConnection(IRoomConnection roomConnection)
        {
            this.roomConnection = roomConnection;
        }

        private void OnEnable()
        {
            SubscribeAll();
        }

        private void OnDisable()
        {
            UnsubscribeAll();
        }

        private void SubscribeAll()
        {
            observable.Subscribe<OnConnectionEvent>(OnClientConnect);
            observable.Subscribe<OnDisconnectionEvent>(OnClientDisconnect);
            //observable.Subscribe<RegisterRoomCommand>(RegisterRoom);
            observable.Subscribe<OnSendEvent>(ReceiveFromClient);
            observable.Subscribe<SendToClientsCommand>(SendToClients);
            observable.Subscribe<StopCommand>(Stop);
        }

        private void UnsubscribeAll()
        {
            observable.Unsubscribe<OnConnectionEvent>(OnClientConnect);
            observable.Unsubscribe<OnDisconnectionEvent>(OnClientDisconnect);
            //observable.Unsubscribe<RegisterRoomCommand>(RegisterRoom);
            observable.Unsubscribe<OnSendEvent>(ReceiveFromClient);
            observable.Unsubscribe<SendToClientsCommand>(SendToClients);
            observable.Unsubscribe<StopCommand>(Stop);
        }

        private void OnClientConnect(OnConnectionEvent e)
        {
            
        }

        private void OnClientDisconnect(OnDisconnectionEvent e)
        {
            
        }

        private void ReceiveFromClient(OnSendEvent e)
        {
            
        }

        private void SendToClients(SendToClientsCommand command)
        {
            
        }

        //private void RegisterRoom(RegisterRoomCommand command)
        //{
            
        //}

        private void Stop(StopCommand command)
        {
            
        }

        private void Disable()
        {
            enabled = false;
        }
    }
}
