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
        private StartMessagingHandler startMessagingHandler;
        private UpdateRoomsHandler updateRoomsHandler;
        private StartNewRoomHandler startNewRoomHandler;
        private UserRoomJoiningAttemptHandler userJoiningAttemptHandler;
        private MyRoomJoiningAttemptHandler myRoomJoiningAttemptHandler;
        private SynchronizationContext synchronization;

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
            parameters = FindObjectOfType<Parameters>();
            startMessagingHandler = GetComponent<StartMessagingHandler>();
            updateRoomsHandler = GetComponent<UpdateRoomsHandler>();
            startNewRoomHandler = GetComponent<StartNewRoomHandler>();
            myRoomJoiningAttemptHandler = GetComponent<MyRoomJoiningAttemptHandler>();
            userJoiningAttemptHandler = GetComponent<UserRoomJoiningAttemptHandler>();
        }

        private void Start()
        {
            synchronization = SynchronizationContext.Current;
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
                startMessagingHandler.Start(roomConnection, value.Id);
                updateRoomsHandler.Start(roomConnection);
                startNewRoomHandler.Start(roomConnection);
                userJoiningAttemptHandler.Start(roomConnection);
                myRoomJoiningAttemptHandler.Start(roomConnection);
                observable.Publish(new OnConnectedToRoomsServiceEvent());
            }, value);
        }
    }
}
