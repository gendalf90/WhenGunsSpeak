using Messages;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    class ClientConnectingState : MonoBehaviour
    {
        [SerializeField]
        private float timeoutInSeconds;

        private Observable observable;
        private Udp udp;
        private SimpleTimer timeoutTimer;
        private ClientProcessingState nextState;
        private StoppingState stoppingState;

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
            udp = GetComponent<Udp>();
            nextState = GetComponent<ClientProcessingState>();
            stoppingState = GetComponent<StoppingState>();
        }

        private void OnEnable()
        {
            StartTimeoutTimer();
            SubscribeAll();
        }

        private void OnDisable()
        {
            UnsubscribeAll();
        }

        private void StartTimeoutTimer()
        {
            timeoutTimer = SimpleTimer.StartNew(timeoutInSeconds);
        }

        private void SubscribeAll()
        {
            observable.Subscribe<OnConnectionEvent>(OnConnection);
        }

        private void UnsubscribeAll()
        {
            observable.Unsubscribe<OnConnectionEvent>(OnConnection);
        }

        private void Update()
        {
            SendPingToServer();
            StopIfConnectTimeout();
        }

        private void StopIfConnectTimeout()
        {
            if (timeoutTimer.ItIsTime)
            {
                Stop();
            }
        }

        private void OnConnection(OnConnectionEvent e)
        {
            if(ServerSession != e.Session)
            {
                return;
            }

            NotifyThatConnected();
            RunNextState();
            Disable();
        }

        public string CurrentSession { get; set; }

        public string ServerSession { get; set; }

        private void Stop()
        {
            RunStopping();
            Disable();
        }

        private void SendPingToServer()
        {
            udp.Send(new Ping(CurrentSession, ServerSession));
        }

        private void RunStopping()
        {
            stoppingState.enabled = true;
        }

        private void RunNextState()
        {
            nextState.CurrentSession = CurrentSession;
            nextState.ServerSession = ServerSession;
            nextState.enabled = true;
        }

        private void NotifyThatConnected()
        {
            observable.Publish(new OnConnectToServerEvent(ServerSession));
        }

        private void Disable()
        {
            enabled = false;
        }
    }
}
