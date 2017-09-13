using Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    class ClientProcessingState : MonoBehaviour
    {
        private Observable observable;
        private Udp udp;

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
            observable.Subscribe<OnDisconnectionEvent>(OnDisconnect);
            observable.Subscribe<OnSendEvent>(ReceiveFromServer);
            observable.Subscribe<SendToServerCommand>(SendToServer);
            observable.Subscribe<StopCommand>(Stop);
        }

        private void UnsubscribeAll()
        {
            observable.Unsubscribe<OnDisconnectionEvent>(OnDisconnect);
            observable.Unsubscribe<OnSendEvent>(ReceiveFromServer);
            observable.Unsubscribe<SendToServerCommand>(SendToServer);
            observable.Unsubscribe<StopCommand>(Stop);
        }

        private void Update()
        {
            SendPingToServer();
        }

        private void OnDisconnect(OnDisconnectionEvent e)
        {
            if (ServerSession == e.Session)
            {
                Stop();
            }
        }

        public string CurrentSession { get; set; }

        public string ServerSession { get; set; }

        private void ReceiveFromServer(OnSendEvent e)
        {
            if (e.FromSession == ServerSession)
            {
                observable.Publish(new OnReceiveEvent(e.FromSession, e.Data));
            }
        }

        private void SendToServer(SendToServerCommand command)
        {
            udp.Send(new SendDecorator(command.Data, CurrentSession, ServerSession));
        }

        private void Stop(StopCommand command)
        {
            Stop();
        }

        private void Stop()
        {
            UnsubscribeAll();
            NotifyThatDisconnected();
            RunStopping();
            Destroy(gameObject);
        }

        private void SendPingToServer()
        {
            udp.Send(new Ping(CurrentSession, ServerSession));
        }

        private void NotifyThatDisconnected()
        {
            observable.Publish(new OnDisconnectFromServerEvent(ServerSession));
        }

        private void RunStopping()
        {
            Instantiate(Resources.Load<GameObject>("Server/States/Stopping"));
        }
    }
}
