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
        private SendHandler receiveFromHandler;
        private ConnectionsHandler connectionsHandler;

        private HashSet<Guid> clients;

        public ServerProcessingState()
        {
            clients = new HashSet<Guid>();
        }

        private void Awake()
        {
            receiveFromHandler = GetComponent<SendHandler>();
            connectionsHandler = GetComponent<ConnectionsHandler>();
            observable = FindObjectOfType<Observable>();
            udp = FindObjectOfType<Udp>();
        }

        private void Start()
        {
            SubscribeAll();
        }

        private void SubscribeAll()
        {
            receiveFromHandler.OnReceiveFrom += Receive;
            connectionsHandler.OnConnect += Connect;
            connectionsHandler.OnDisconnect += Disconnect;
            observable.Subscribe<RegisterCommand>(Register);
            observable.Subscribe<SendToCommand>(SendTo);
            observable.Subscribe<SendToClientsCommand>(SendToClients);
            observable.Subscribe<StopCommand>(Stop);
        }

        private void UnsubscribeAll()
        {
            receiveFromHandler.OnReceiveFrom -= Receive;
            connectionsHandler.OnConnect -= Connect;
            connectionsHandler.OnDisconnect -= Disconnect;
            observable.Unsubscribe<RegisterCommand>(Register);
            observable.Unsubscribe<SendToCommand>(SendTo);
            observable.Unsubscribe<SendToClientsCommand>(SendToClients);
            observable.Unsubscribe<StopCommand>(Stop);
        }

        public string CurrentSession { get; set; }

        private void Receive(object sender, ReceiveFromEventArgs args)
        {
            if (clients.Contains(args.From))
            {
                observable.Publish(new OnReceiveEvent(args.From, args.Data));
            }
        }

        private void Connect(object sender, ConnectionEventArgs args)
        {
            if (clients.Count == maxConnections)
            {
                return;
            }

            clients.Add(args.Guid);
            observable.Publish(new OnClientConnectEvent(args.Guid));
        }

        private void Disconnect(object sender, ConnectionEventArgs args)
        {
            if (clients.Remove(args.Guid))
            {
                observable.Publish(new OnClientDisconnectEvent(args.Guid));
            }
        }

        private void SendTo(SendToCommand command)
        {
            udp.Send(new SendDecorator(command.Data, Guid, command.To));
        }

        private void SendToClients(SendToClientsCommand command)
        {
            udp.Send(new SendDecorator(command.Data, Guid, clients));
        }

        private void Register(RegisterCommand command)
        {
            udp.Send(new Registration(Guid, command.Description));
        }

        private void Stop(StopCommand command)
        {
            UnsubscribeAll();
            RunStopping();
            Destroy(gameObject);
        }

        private void RunStopping()
        {
            Instantiate(Resources.Load<GameObject>("Server/States/Stopping"));
        }
    }
}
