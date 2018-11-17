using Connection;
using Messages;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

namespace Server
{
    class ConnectedState : MonoBehaviour
    {
        private Observable observable;
        private IRoomConnection roomConnection;

        private ThreadLocker threadLocker;
        private OnAllRoomsUpdatedEvent allRoomsUpdatedEvent;
        private OnRoomUpdatedEvent roomUpdatedEvent;

        public ConnectedState()
        {
            threadLocker = new ThreadLocker();
        }

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

        private void Update()
        {
            SendAllRoomsUpdatedEvent();
            SendRoomUpdatedEvent();
        }

        private void SendAllRoomsUpdatedEvent()
        {
            threadLocker.Do(() =>
            {
                if(allRoomsUpdatedEvent == null)
                {
                    return;
                }

                observable.Publish(allRoomsUpdatedEvent);
                allRoomsUpdatedEvent = null;
            });
        }

        private void SendRoomUpdatedEvent()
        {
            threadLocker.Do(() =>
            {
                if (roomUpdatedEvent == null)
                {
                    return;
                }

                observable.Publish(roomUpdatedEvent);
                roomUpdatedEvent = null;
            });
        }

        private void SubscribeAll()
        {
            observable.Subscribe<RefreshAllRoomsCommand>(RefreshAllRooms);
            observable.Subscribe<RefreshRoomCommand>(RefreshRoom);
        }

        private void UnsubscribeAll()
        {
            observable.Unsubscribe<RefreshAllRoomsCommand>(RefreshAllRooms);
            observable.Unsubscribe<RefreshRoomCommand>(RefreshRoom);
        }

        private void RefreshAllRooms(RefreshAllRoomsCommand command)
        {
            roomConnection.GetAllRoomsAsync().ContinueWith(data =>
            {
                CreateRoomsUpdatedEvent(data.Result);
            });
        }

        private void CreateRoomsUpdatedEvent(IEnumerable<RoomData> allRooms)
        {
            threadLocker.Do(() =>
            {
                var allRoomsData = allRooms.Select(room => new RoomShortData(room.OwnerId, room.Header));
                allRoomsUpdatedEvent = new OnAllRoomsUpdatedEvent(allRoomsData);
            });
        }

        private void RefreshRoom(RefreshRoomCommand command)
        {
            roomConnection.GetRoomDescriptionAsync(command.OwnerId).ContinueWith(data =>
            {
                CreateRoomUpdatedEvent(data.Result);
            });
        }

        private void CreateRoomUpdatedEvent(RoomDescriptionData data)
        {
            threadLocker.Do(() =>
            {
                roomUpdatedEvent = new OnRoomUpdatedEvent(data.OwnerId, data.Header, data.Description);
            });
        }

        public void SetRoomConnection(IRoomConnection connection)
        {
            roomConnection = connection;
        }

        private void Disable()
        {
            enabled = false;
        }
    }
}
