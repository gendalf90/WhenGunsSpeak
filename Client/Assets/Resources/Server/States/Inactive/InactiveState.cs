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
        private GameObject nextStatePrefab;

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
        }

        private void Start()
        {
            SubscribeAll();
        }

        private void SubscribeAll()
        {
            observable.Subscribe<StartAsServerCommand>(OnServerStartEventHandler);
            observable.Subscribe<StartAsClientCommand>(OnClientStartEventHandler);
            observable.Subscribe<StartRoomsListenCommand>(OnRoomsListenStartEventHandler);
        }

        private void UnsubscribeAll()
        {
            observable.Unsubscribe<StartAsServerCommand>(OnServerStartEventHandler);
            observable.Unsubscribe<StartAsClientCommand>(OnClientStartEventHandler);
            observable.Unsubscribe<StartRoomsListenCommand>(OnRoomsListenStartEventHandler);
        }

        private void OnRoomsListenStartEventHandler(StartRoomsListenCommand command)
        {
            UnsubscribeAll();
            StartUdp();
            ConfigureNextStateAsRoomsListener();
            StartNextState();
            Destroy(gameObject);
        }

        private void OnClientStartEventHandler(StartAsClientCommand command)
        {
            UnsubscribeAll();
            StartUdp();
            ConfigureNextStateAsClient(command.ConnectToSession);
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

        private void ConfigureNextStateAsRoomsListener()
        {
            nextStatePrefab = Resources.Load<GameObject>("Server/States/RoomsListening");
        }

        private void ConfigureNextStateAsClient(string serverSession)
        {
            nextStatePrefab = Resources.Load<GameObject>("Server/States/GetSession");
            var nextState = nextStatePrefab.GetComponent<GetSessionState>();
            nextState.SetAsClient(serverSession);

        }

        private void ConfigureNextStateAsServer()
        {
            nextStatePrefab = Resources.Load<GameObject>("Server/States/GetSession");
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
