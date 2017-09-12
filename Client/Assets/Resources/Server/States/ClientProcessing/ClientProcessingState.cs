using Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    class ClientProcessingState : MonoBehaviour
    {
        [SerializeField]
        private float timeoutInSeconds;

        private Observable observable;
        private Udp udp;
        private SendHandler receiveFromHandler;
        private ConnectionsHandler connectionsHandler;

        private bool isConnect;
        private bool isStopped;
        private SimpleTimer connectTimeoutTimer;

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
            udp = FindObjectOfType<Udp>();
            receiveFromHandler = GetComponent<SendHandler>();
            connectionsHandler = GetComponent<ConnectionsHandler>();
        }

        private void Start()
        {
            StartTimeoutTimer();
            SubscribeAll();
        }

        private void StartTimeoutTimer()
        {
            connectTimeoutTimer = SimpleTimer.StartNew(timeoutInSeconds);
        }

        private void SubscribeAll()
        {
            receiveFromHandler.OnReceiveFrom += Receive;
            connectionsHandler.OnConnect += Connect;
            connectionsHandler.OnDisconnect += Disconnect;
            observable.Subscribe<SendToCommand>(SendTo);
            observable.Subscribe<SendToServerCommand>(SendToServer);
            observable.Subscribe<StopCommand>(Stop);
        }

        private void UnsubscribeAll()
        {
            receiveFromHandler.OnReceiveFrom -= Receive;
            connectionsHandler.OnConnect -= Connect;
            connectionsHandler.OnDisconnect -= Disconnect;
            observable.Unsubscribe<SendToCommand>(SendTo);
            observable.Unsubscribe<SendToServerCommand>(SendToServer);
            observable.Unsubscribe<StopCommand>(Stop);
        }

        private void Update()
        {
            SendPingToServer();
            StopIfConnectTimeout();
        }

        private void StopIfConnectTimeout()
        {
            if (!isConnect && connectTimeoutTimer.ItIsTime)
            {
                Stop();
            }
        }

        public string CurrentSession { get; set; }

        public string ServerSession { get; set; }

        private void SendTo(SendToCommand command)
        {
            udp.Send(new SendDecorator(command.Data, MyGuid, command.To));
        }

        private void SendToServer(SendToServerCommand command)
        {
            udp.Send(new SendDecorator(command.Data, MyGuid, ServerGuid));
        }

        private void Stop(StopCommand command)
        {
            Stop();
        }

        private void Stop()
        {
            if (!TrySetStopped())
            {
                return;
            }

            UnsubscribeAll();
            RunStopping();
            Destroy(gameObject);
        }

        private bool TrySetStopped()
        {
            if (isStopped)
            {
                return false;
            }

            isStopped = true;
            return true;
        }

        private void Receive(object sender, ReceiveFromEventArgs args)
        {
            if (isConnect && args.From == ServerGuid)
            {
                observable.Publish(new OnReceiveEvent(args.From, args.Data));
            }
        }

        private void Connect(object sender, ConnectionEventArgs args)
        {
            if (args.Guid != ServerGuid)
            {
                return;
            }

            isConnect = true;
            observable.Publish(new OnConnectToServerEvent(ServerGuid));
        }

        private void Disconnect(object sender, ConnectionEventArgs args)
        {
            if (args.Guid != ServerGuid)
            {
                return;
            }

            observable.Publish(new OnDisconnectEvent(ServerGuid));
            connectTimeoutTimer.Restart();
            isConnect = false;
        }

        private void SendPingToServer()
        {
            udp.Send(new Ping(MyGuid, ServerGuid));
        }

        private void RunStopping()
        {
            Instantiate(Resources.Load<GameObject>("Server/States/Stopping"));
        }
    }
}
