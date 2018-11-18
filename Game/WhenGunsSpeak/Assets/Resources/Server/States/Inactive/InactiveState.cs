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

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
        }

        private void OnEnable()
        {
            SubscribeAll();
        }

        private void OnDisable()
        {
            UnsubscribeAll();
        }

        private void SubscribeAll()
        {
            //observable.Subscribe<StartAsServerCommand>(OnServerStartEventHandler);
            observable.Subscribe<StartAsClientCommand>(OnClientStartEventHandler);
            observable.Subscribe<StartRoomsListenCommand>(OnRoomsListenStartEventHandler);
        }

        private void UnsubscribeAll()
        {
            //observable.Unsubscribe<StartAsServerCommand>(OnServerStartEventHandler);
            observable.Unsubscribe<StartAsClientCommand>(OnClientStartEventHandler);
            observable.Unsubscribe<StartRoomsListenCommand>(OnRoomsListenStartEventHandler);
        }

        private void OnRoomsListenStartEventHandler(StartRoomsListenCommand command)
        {
            StartUdp();
            StartNextStateAsRoomsListener();
            Disable();
        }

        private void OnClientStartEventHandler(StartAsClientCommand command)
        {
            StartUdp();
            StartNextStateAsClient(command.ConnectToSession);
            Disable();
        }

        //private void OnServerStartEventHandler(StartAsServerCommand command)
        //{
        //    StartUdp();
        //    StartNextStateAsServer();
        //    Disable();
        //}

        private void StartNextStateAsRoomsListener()
        {
            var state = GetComponent<RoomsListeningState>();
            state.enabled = true;
        }

        private void StartNextStateAsClient(string serverSession)
        {
            var state = GetComponent<GetSessionState>();
            state.SetAsClient(serverSession);
            state.enabled = true;

        }

        private void StartNextStateAsServer()
        {
            var state = GetComponent<GetSessionState>();
            state.SetAsServer();
            state.enabled = true;
        }

        private void StartUdp()
        {
            var udp = GetComponent<Udp>();
            udp.enabled = true;
        }

        private void Disable()
        {
            enabled = false;
        }
    }
}
