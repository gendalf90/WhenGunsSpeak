using Connection;
using Messages;
using System;
using UnityEngine;

namespace Server
{
    class NotConnectedState : MonoBehaviour
    {
        private Observable observable;
        private volatile IRoomConnection roomConnection;

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
            observable.Subscribe<ConnectCommand>(ConnectHandler);
        }

        private void UnsubscribeAll()
        {
            observable.Unsubscribe<ConnectCommand>(ConnectHandler);
        }

        private void ConnectHandler(ConnectCommand command)
        {
            StartConnection();
        }

        private void Update()
        {
            if(!ConnectionReady)
            {
                return;
            }

            SendConnectEvent();
            StartConnectedState();
            Disable();
        }

        private void StartConnection()
        {
            new Bootstrap().StartRoomConnectionAsync(new RoomConnectionOptions
            {
                RoomsAddress = new Uri("http://localhost:50557")
            }).ContinueWith(result =>
            {
                roomConnection = result.Result;
            });
        }

        private void StartConnectedState()
        {
            var state = GetComponent<ConnectedState>();
            state.SetRoomConnection(roomConnection);
            state.enabled = true;
        }

        private void SendConnectEvent()
        {
            observable.Publish(new OnConnectEvent());
        }

        private bool ConnectionReady => roomConnection != null;

        private void Disable()
        {
            enabled = false;
        }
    }
}
