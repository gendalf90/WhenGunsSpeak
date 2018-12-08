using Configuration;
using Connection;
using Messages;
using System;
using System.Threading;
using UnityEngine;
using Utils;

namespace Server
{
    class StartRoomConnectionHandler : MonoBehaviour
    {
        private IRoomConnection roomConnection;
        private Observable observable;
        private Parameters parameters;
        private SynchronizationContext synchronization;

        private void Awake()
        {
            synchronization = SynchronizationContext.Current;
            observable = FindObjectOfType<Observable>();
            parameters = FindObjectOfType<Parameters>();
        }

        private void OnEnable()
        {
            observable.Subscribe<ConnectToRoomsServiceCommand>(StartHandle);
        }

        private async void StartHandle(ConnectToRoomsServiceCommand command)
        {
            roomConnection = await new Bootstrap().StartRoomConnectionAsync(new RoomConnectionOptions
            {
                RoomsAddress = parameters.GetLocalOrDefault<Uri>("RoomsServiceAddress")
            });
            roomConnection.Subscribe<MyData>(next: MyDataHandler);
            await roomConnection.KnowAboutMeAsync();
        }

        private void MyDataHandler(MyData value)
        {
            synchronization.Post(state =>
            {
                OnConnected?.Invoke(this, new StartRoomConnectionEventArgs(roomConnection, value.Id));
                observable.Publish(new OnConnectedToRoomsServiceEvent());
            }, value);
        }

        public event EventHandler<StartRoomConnectionEventArgs> OnConnected;
    }
}
