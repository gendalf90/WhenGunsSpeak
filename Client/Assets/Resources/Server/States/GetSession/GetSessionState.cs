using Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    class GetSessionState : MonoBehaviour
    {
        [SerializeField]
        private float timeoutInSeconds;

        private Observable observable;
        private Udp udp;
        private SimpleTimer timeoutTimer;
        private GameObject nextStatePrefab;

        private Action onStartedNotificationStrategy;
        private Action<string> setCurrentSessionStrategy;

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
            udp = FindObjectOfType<Udp>();
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

        private void Update()
        {
            SendRequestToGetSession();
            StopIfTimeout();
        }

        private void SendRequestToGetSession()
        {
            udp.Send(new Session());
        }

        private void StopIfTimeout()
        {
            if (timeoutTimer.ItIsTime)
            {
                Stop();
            }
        }

        private void Stop()
        {
            UnsubscribeAll();
            RunStopping();
            Destroy(gameObject);
        }

        private void SubscribeAll()
        {
            observable.Subscribe<OnSessionEvent>(OnGetSession);
        }

        private void UnsubscribeAll()
        {
            observable.Unsubscribe<OnSessionEvent>(OnGetSession);
        }

        private void OnGetSession(OnSessionEvent e)
        {
            UnsubscribeAll();
            InitializeEvent(e);
            NotifyThatHasStarted();
            StartNextState();
            Destroy(gameObject);
        }

        private void InitializeEvent(OnSessionEvent e)
        {
            setCurrentSessionStrategy(e.Session);
        }

        public void SetAsClient(string serverSession)
        {
            nextStatePrefab = Resources.Load<GameObject>("Server/States/ClientConnecting");
            var nextState = nextStatePrefab.GetComponent<ClientConnectingState>();
            nextState.ServerSession = serverSession;
            setCurrentSessionStrategy = currentSession => nextState.CurrentSession = currentSession;
            onStartedNotificationStrategy = () => observable.Publish(new OnStartedAsClientEvent(nextState.CurrentSession));
        }

        public void SetAsServer()
        {
            nextStatePrefab = Resources.Load<GameObject>("Server/States/ServerProcessing");
            var nextState = nextStatePrefab.GetComponent<ServerProcessingState>();
            setCurrentSessionStrategy = currentSession => nextState.CurrentSession = currentSession;
            onStartedNotificationStrategy = () => observable.Publish(new OnStartedAsServerEvent(nextState.CurrentSession));
        }

        private void StartNextState()
        {
            Instantiate(nextStatePrefab);
        }

        private void NotifyThatHasStarted()
        {
            onStartedNotificationStrategy();
        }

        private void RunStopping()
        {
            Instantiate(Resources.Load<GameObject>("Server/States/Stopping"));
        }
    }
}
