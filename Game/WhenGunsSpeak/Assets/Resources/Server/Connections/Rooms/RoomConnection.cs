using Configuration;
using Connection;
using Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace Server
{
    class RoomConnection : MonoBehaviour, 
                           IObserver<MyData>, 
                           IObserver<UserIPEvent>,
                           IObserver<RoomJoiningData>
    {
        private Observable observable;
        private Parameters parameters;

        private IRoomConnection roomConnection;
        private List<IDisposable> unsubscribers;
        private List<Guid> userIds;
        private SynchronizationContext synchronization;

        public RoomConnection()
        {
            unsubscribers = new List<IDisposable>();
            userIds = new List<Guid>();
        }

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
            parameters = FindObjectOfType<Parameters>();
        }

        private void Start()
        {
            synchronization = SynchronizationContext.Current;
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
            observable.Subscribe<KnowAboutMeCommand>(KnowAboutMeHandler);
            observable.Subscribe<StartUserToRoomJoiningCommand>(StartUserToRoomJoiningHandler);
        }

        private void UnsubscribeAll()
        {
            observable.Unsubscribe<StartRoomConnectionCommand>(Connect);
            observable.Unsubscribe<StartRoomsUpdatingCommand>(StartRoomsUpdating);
            observable.Unsubscribe<StartRoomCommand>(StartRoomHandler);
            observable.Unsubscribe<KnowAboutMeCommand>(KnowAboutMeHandler);
            observable.Unsubscribe<StartUserToRoomJoiningCommand>(StartUserToRoomJoiningHandler);
        }

        private async void Connect(StartRoomConnectionCommand command)
        {
            roomConnection = await new Bootstrap().StartRoomConnectionAsync(new RoomConnectionOptions
            {
                RoomsAddress = parameters.GetLocalOrDefault<Uri>("RoomsServiceAddress")
            });

            unsubscribers.Add(roomConnection.Subscribe((IObserver<MyData>)this));

            IsConnected = true;
            observable.Publish(new OnConnectedToRoomsServiceEvent());
        }

        private async void StartRoomsUpdating(StartRoomsUpdatingCommand command)
        {
            if(!IsConnected)
            {
                return;
            }

            var rooms = await roomConnection.GetAllRoomsAsync();
            var roomsShortData = rooms.Select(room => new RoomShortData(room.OwnerId, room.Header));
            observable.Publish(new OnAllRoomsUpdatedEvent(roomsShortData));
        }

        private async void StartRoomHandler(StartRoomCommand command)
        {
            if(!IsConnected)
            {
                return;
            }

            await roomConnection.CreateMyRoomAsync(command.Header);
            observable.Publish(new OnNewRoomStartedEvent());
        }

        private async void KnowAboutMeHandler(KnowAboutMeCommand command)
        {
            if (!IsConnected)
            {
                return;
            }

            await roomConnection.KnowAboutMeAsync();
        }

        private async void MyIPHandler(MyIPEvent e)
        {
            if (!IsConnected)
            {
                return;
            }

            var toSendUserIds = userIds.ToArray();

            foreach(var clientId in toSendUserIds)
            {
                await roomConnection.TellMyIpAsync(clientId, e.MyIp.Address, e.MyIp.Port);
            }
        }

        private async void StartUserToRoomJoiningHandler(StartUserToRoomJoiningCommand command)
        {
            if (!IsConnected)
            {
                return;
            }

            userIds.Add(command.UserId);
            await roomConnection.JoinTheUserToMyRoomAsync(command.UserId);
        }

        private bool IsConnected { get; set; }

        private void OnDestroy()
        {
            unsubscribers.ForEach(unsubscriber =>
            {
                unsubscriber.Dispose();
            });

            roomConnection?.Dispose();
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        void IObserver<MyData>.OnNext(MyData value)
        {
            synchronization.Post(state =>
            {
                observable.Publish(new AboutMeEvent(value.Id));
            }, value);
        }

        void IObserver<UserIPEvent>.OnNext(UserIPEvent value)
        {
            synchronization.Post(state =>
            {
                observable.Publish(new UserIPEvent(value.UserId, value.UserIp));
            }, value);
        }

        void IObserver<RoomJoiningData>.OnNext(RoomJoiningData value)
        {
            synchronization.Post(state =>
            {
                observable.Publish(new UserWantsToJoinToRoomEvent(value.UserId));
            }, value);
        }
    }
}
