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
        private Guid connectingGuid;
        private Guid startingGuid;
        private Role startingRole;

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
            StartClient();
            NotifyThatHasStarted();
            Destroy(gameObject);
        }

        private void OnServerStartEventHandler(StartAsServerCommand command)
        {
            UnsubscribeAll();
            InitializeCommand(command);
            StartUdp();
            StartServer();
            NotifyThatHasStarted();
            Destroy(gameObject);
        }

        private void InitializeCommand(StartAsClientCommand command)
        {
            connectingGuid = command.ConnectTo;
            startingGuid = command.Guid;
            startingRole = Role.Client;
        }

        private void InitializeCommand(StartAsServerCommand command)
        {
            startingGuid = command.Guid;
            startingRole = Role.Server;
        }

        private void StartServer()
        {
            var prefab = Resources.Load<GameObject>("Server/States/ServerProcessing");
            var state = prefab.GetComponent<ServerProcessingState>();
            state.Guid = startingGuid;
            Instantiate(prefab);
        }

        private void StartClient()
        {
            var prefab = Resources.Load<GameObject>("Server/States/ClientProcessing");
            var state = prefab.GetComponent<ClientProcessingState>();
            state.MyGuid = startingGuid;
            state.ServerGuid = connectingGuid;
            Instantiate(prefab);
        }

        private void StartUdp()
        {
            Instantiate(Resources.Load<GameObject>("Server/Udp/Udp"));
        }

        private void NotifyThatHasStarted()
        {
            observable.Publish(new OnStartedEvent(startingGuid, startingRole));
        }
    }
}
