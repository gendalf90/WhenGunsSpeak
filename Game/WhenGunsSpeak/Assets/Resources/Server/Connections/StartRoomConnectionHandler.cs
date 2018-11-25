using Configuration;
using Connection;
using Messages;
using System;
using UnityEngine;

namespace Server
{
    class StartRoomConnectionHandler : MonoBehaviour
    {
        private Observable observable;
        private Parameters parameters;

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
            parameters = FindObjectOfType<Parameters>();
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
            observable.Subscribe<StartRoomConnectionCommand>(Start);
        }

        private void UnsubscribeAll()
        {
            observable.Unsubscribe<StartRoomConnectionCommand>(Start);
        }

        private async void Start(StartRoomConnectionCommand command)
        {
            var connection = await new Bootstrap().StartRoomConnectionAsync(new RoomConnectionOptions
            {
                RoomsAddress = parameters.GetLocalOrDefault<Uri>("RoomsServiceAddress")
            });

            observable.Publish(new OnConnectedToRoomsServiceEvent());
        }
    }
}
