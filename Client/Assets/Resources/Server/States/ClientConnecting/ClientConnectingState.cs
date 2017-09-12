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
        private GameObject nextStatePrefab;

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
            udp = FindObjectOfType<Udp>();
            nextStatePrefab = Resources.Load<GameObject>("Server/States/ClientProcessing");
        }

        private void Start()
        {
            StartTimeoutTimer();
            SubscribeAll();
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

            UnsubscribeAll();
            NotifyThatConnected();
            RunNextState();
            Destroy(gameObject);
        }

        public string CurrentSession { get; set; }

        public string ServerSession { get; set; }

        private void Stop()
        {
            UnsubscribeAll();
            RunStopping();
            Destroy(gameObject);
        }

        private void SendPingToServer()
        {
            udp.Send(new Ping(CurrentSession, ServerSession));
        }

        private void RunStopping()
        {
            Instantiate(Resources.Load<GameObject>("Server/States/Stopping"));
        }

        private void RunNextState()
        {
            var nextState = nextStatePrefab.GetComponent<ClientProcessingState>();
            nextState.CurrentSession = CurrentSession;
            nextState.ServerSession = ServerSession;
            Instantiate(nextStatePrefab);
        }

        private void NotifyThatConnected()
        {
            observable.Publish(new OnConnectToServerEvent(ServerSession));
        }
    }
}
