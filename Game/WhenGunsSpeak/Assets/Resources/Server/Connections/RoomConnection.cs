using Configuration;
using Connection;
using Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

namespace Server
{
    class RoomConnection : MonoBehaviour
    {
        private Observable observable;
        private Parameters parameters;
        private volatile IRoomConnection roomConnection;
        private volatile bool hasConnected;
        private ThreadLocker threadLocker;
        private List<RoomShortData> allRoomsBuffer;
        private volatile bool isNewRoomHasStarted;

        public RoomConnection()
        {
            threadLocker = new ThreadLocker();
            allRoomsBuffer = new List<RoomShortData>();
        }

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

        public void Connect(StartRoomConnectionCommand command)
        {
            new Bootstrap().StartRoomConnectionAsync(new RoomConnectionOptions
            {
                RoomsAddress = new Uri(parameters.GetLocalOrDefault<string>("RoomsServiceAddress"))
            }).ContinueWith(result =>
            {
                roomConnection = result.Result;
                hasConnected = true;
            });
        }

        private void Update()
        {
            SendConnectEventIfHasConnected();
            SendFinishRoomsUpdatingEventIfAny();
            SendIfNewRoomHasStarted();
        }

        private void SendConnectEventIfHasConnected()
        {
            if(!hasConnected)
            {
                return;
            }

            observable.Publish(new OnConnectEvent());
            hasConnected = false;
        }

        private void SendFinishRoomsUpdatingEventIfAny()
        {
            threadLocker.Do(() =>
            {
                if(!allRoomsBuffer.Any())
                {
                    return;
                }

                observable.Publish(new OnAllRoomsUpdatedEvent(allRoomsBuffer));
                allRoomsBuffer.Clear();
            });
        }

        private void SendIfNewRoomHasStarted()
        {
            if(isNewRoomHasStarted)
            {
                observable.Publish(new OnNewRoomHasStartedEvent());
                isNewRoomHasStarted = false;
            }
        }

        private void StartRoomsUpdating(StartRoomsUpdatingCommand command)
        {
            roomConnection?.GetAllRoomsAsync().ContinueWith(data =>
            {
                FillRoomsBuffer(data.Result);
            });
        }

        private void FillRoomsBuffer(IEnumerable<RoomData> allRooms)
        {
            threadLocker.Do(() =>
            {
                allRoomsBuffer.Clear();
                allRoomsBuffer.AddRange(allRooms.Select(room => new RoomShortData(room.OwnerId, room.Header)));
            });
        }

        private void StartRoomHandler(StartRoomCommand command)
        {
            roomConnection?.CreateMyRoomAsync(command.Header).ContinueWith(data =>
            {
                isNewRoomHasStarted = true;
            });
        }

        private void OnDestroy()
        {
            roomConnection?.Dispose();
        }
    }
}
