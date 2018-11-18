using Configuration;
using Connection;
using Messages;
using System;
using System.Linq;
using UnityEngine;

namespace Server
{
    class RoomConnection : MonoBehaviour
    {
        private Observable observable;
        private Parameters parameters;

        private IRoomConnection roomConnection;

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
            observable.Subscribe<StartRoomConnectionCommand>(Connect);
            observable.Subscribe<StartRoomsUpdatingCommand>(StartRoomsUpdating);
            observable.Subscribe<StartRoomCommand>(StartRoomHandler);
        }

        private void UnsubscribeAll()
        {
            observable.Unsubscribe<StartRoomConnectionCommand>(Connect);
            observable.Unsubscribe<StartRoomsUpdatingCommand>(StartRoomsUpdating);
            observable.Unsubscribe<StartRoomCommand>(StartRoomHandler);
        }

        private async void Connect(StartRoomConnectionCommand command)
        {
            roomConnection = await new Bootstrap().StartRoomConnectionAsync(new RoomConnectionOptions
            {
                RoomsAddress = new Uri(parameters.GetLocalOrDefault<string>("RoomsServiceAddress"))
            });

            observable.Publish(new OnConnectEvent());
        }

        private async void StartRoomsUpdating(StartRoomsUpdatingCommand command)
        {
            if(roomConnection == null)
            {
                return;
            }

            var rooms = await roomConnection.GetAllRoomsAsync();
            var roomsShortData = rooms.Select(room => new RoomShortData(room.OwnerId, room.Header));
            observable.Publish(new OnAllRoomsUpdatedEvent(roomsShortData));
        }

        private async void StartRoomHandler(StartRoomCommand command)
        {
            if(roomConnection == null)
            {
                return;
            }

            await roomConnection.CreateMyRoomAsync(command.Header);
            observable.Publish(new OnNewRoomHasStartedEvent());
        }

        private void OnDestroy()
        {
            roomConnection?.Dispose();
        }
    }
}
