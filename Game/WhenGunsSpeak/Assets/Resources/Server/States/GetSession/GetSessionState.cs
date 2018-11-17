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

        private Action<string> startNextStateStrategy;

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
            udp = GetComponent<Udp>();
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
            RunStopping();
            Disable();
        }

        private void Disable()
        {
            enabled = false;
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
            StartNextState(e.Session);
            Disable();
        }

        public void SetAsClient(string serverSession)
        {
            var nextState = GetComponent<ClientConnectingState>();
            startNextStateStrategy = currentSession =>
            {
                nextState.ServerSession = serverSession;
                nextState.CurrentSession = currentSession;
                observable.Publish(new OnStartedAsClientEvent(nextState.CurrentSession));
                nextState.enabled = true;
            };
        }

        public void SetAsServer()
        {
            var nextState = GetComponent<ServerProcessingState>();
            startNextStateStrategy = currentSession =>
            {
                //nextState.CurrentSession = currentSession;
                //observable.Publish(new OnStartedAsServerEvent(nextState.CurrentSession));
                nextState.enabled = true;
            };
        }

        private void StartNextState(string currentSession)
        {
            startNextStateStrategy(currentSession);
        }

        private void RunStopping()
        {
            var state = GetComponent<StoppingState>();
            state.enabled = true;
        }
    }
}
