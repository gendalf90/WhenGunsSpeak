using Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    class ServerProcessingState : MonoBehaviour
    {
        [SerializeField]
        private int maxConnections;

        private Observable observable;
        private Udp udp;

        private HashSet<string> clients;

        public ServerProcessingState()
        {
            clients = new HashSet<string>();
        }

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
            udp = FindObjectOfType<Udp>();
        }

        private void Start()
        {
            SubscribeAll();
        }

        private void SubscribeAll()
        {
            observable.Subscribe<OnConnectionEvent>(OnClientConnect);
            observable.Subscribe<OnDisconnectionEvent>(OnClientDisconnect);
            observable.Subscribe<RegisterRoomCommand>(Register);
            observable.Subscribe<OnSendEvent>(ReceiveFromClient);
            observable.Subscribe<SendToClientsCommand>(SendToClients);
            observable.Subscribe<StopCommand>(Stop);
        }

        private void UnsubscribeAll()
        {
            observable.Unsubscribe<OnConnectionEvent>(OnClientConnect);
            observable.Unsubscribe<OnDisconnectionEvent>(OnClientDisconnect);
            observable.Unsubscribe<RegisterRoomCommand>(Register);
            observable.Unsubscribe<OnSendEvent>(ReceiveFromClient);
            observable.Unsubscribe<SendToClientsCommand>(SendToClients);
            observable.Unsubscribe<StopCommand>(Stop);
        }

        public string CurrentSession { get; set; }

        private void OnClientConnect(OnConnectionEvent e)
        {
            if(clients.Count == maxConnections)
            {
                return;
            }

            clients.Add(e.Session);
            observable.Publish(new OnClientConnectEvent(e.Session));
        }

        private void OnClientDisconnect(OnDisconnectionEvent e)
        {
            if(!clients.Contains(e.Session))
            {
                return;
            }

            clients.Remove(e.Session);
            observable.Publish(new OnClientDisconnectEvent(e.Session));
        }

        private void ReceiveFromClient(OnSendEvent e)
        {
            if(clients.Contains(e.FromSession))
            {
                observable.Publish(new OnReceiveEvent(e.FromSession, e.Data));
            }
        }

        private void SendToClients(SendToClientsCommand command)
        {
            udp.Send(new SendDecorator(command.Data, CurrentSession, clients));
        }

        private void Register(RegisterRoomCommand command)
        {
            udp.Send(new Room(CurrentSession, command.Description));
        }

        private void Stop(StopCommand command)
        {
            UnsubscribeAll();
            NotifyThatAllClientsDisconnected();
            RunStopping();
            Destroy(gameObject);
        }

        private void NotifyThatAllClientsDisconnected()
        {
            foreach(var session in clients)
            {
                observable.Publish(new OnClientDisconnectEvent(session));
            }
        }

        private void RunStopping()
        {
            Instantiate(Resources.Load<GameObject>("Server/States/Stopping"));
        }
    }
}
