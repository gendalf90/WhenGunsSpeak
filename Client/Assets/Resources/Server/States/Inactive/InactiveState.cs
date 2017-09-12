using Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    class InactiveState : MonoBehaviour
    {
        private Observable observable;
        private string clientConnectingSession;
        private GameObject nextStatePrefab;

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
            nextStatePrefab = Resources.Load<GameObject>("Server/States/GetSession");
        }

        private void Start()
        {
            SubscribeAll();
        }

        private void SubscribeAll()
        {
            observable.Subscribe<StartAsServerCommand>(OnServerStartEventHandler);
            observable.Subscribe<StartAsClientCommand>(OnClientStartEventHandler);
        }

        private void UnsubscribeAll()
        {
            observable.Unsubscribe<StartAsServerCommand>(OnServerStartEventHandler);
            observable.Unsubscribe<StartAsClientCommand>(OnClientStartEventHandler);
        }

        private void OnClientStartEventHandler(StartAsClientCommand command)
        {
            UnsubscribeAll();
            InitializeCommand(command);
            StartUdp();
            ConfigureNextStateAsClient();
            StartNextState();
            Destroy(gameObject);
        }

        private void OnServerStartEventHandler(StartAsServerCommand command)
        {
            UnsubscribeAll();
            StartUdp();
            ConfigureNextStateAsServer();
            StartNextState();
            Destroy(gameObject);
        }

        private void InitializeCommand(StartAsClientCommand command)
        {
            clientConnectingSession = command.ConnectToSession;
        }

        private void ConfigureNextStateAsClient()
        {
            var nextState = nextStatePrefab.GetComponent<GetSessionState>();
            nextState.SetAsClient(clientConnectingSession);

        }

        private void ConfigureNextStateAsServer()
        {
            var nextState = nextStatePrefab.GetComponent<GetSessionState>();
            nextState.SetAsServer();
        }

        private void StartNextState()
        {
            Instantiate(nextStatePrefab);
        }

        private void StartUdp()
        {
            Instantiate(Resources.Load<GameObject>("Server/Udp/Udp"));
        }
    }
}
