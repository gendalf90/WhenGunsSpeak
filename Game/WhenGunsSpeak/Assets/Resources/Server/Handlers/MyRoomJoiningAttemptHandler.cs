using Connection;
using Messages;
using System.Threading;
using UnityEngine;
using Utils;

namespace Server
{
    class MyRoomJoiningAttemptHandler : MonoBehaviour
    {
        private IRoomConnection roomConnection;
        private Observable observable;
        private SynchronizationContext synchronization;
        private StartRoomConnectionHandler roomsConnectionHandler;

        private void Awake()
        {
            synchronization = SynchronizationContext.Current;
            observable = FindObjectOfType<Observable>();
            roomsConnectionHandler = GetComponent<StartRoomConnectionHandler>();
        }

        private void OnEnable()
        {
            roomsConnectionHandler.OnConnected += ConnectionHandler_OnConnected;
        }

        private void ConnectionHandler_OnConnected(object sender, StartRoomConnectionEventArgs e)
        {
            roomConnection = e.RoomConnection;
            roomConnection.Subscribe<RoomJoinedData>(next: AcceptMyJoiningHandle);
            roomConnection.Subscribe<RoomRejectedData>(next: DeclineMyJoiningHandle);
            observable.Subscribe<JoinMeToRoomCommand>(JoinMeToRoomHandle);
        }

        private void AcceptMyJoiningHandle(RoomJoinedData value)
        {
            synchronization.Post(state =>
            {
                observable.Publish(new IAmJoinedToRoomEvent(value.OwnerId));
            }, value);
        }

        private void DeclineMyJoiningHandle(RoomRejectedData value)
        {
            synchronization.Post(state =>
            {
                observable.Publish(new IAmNotJoinedToRoomEvent(value.OwnerId, value.Message));
            }, value);
        }

        private async void JoinMeToRoomHandle(JoinMeToRoomCommand command)
        {
            await roomConnection.AskToJoinToHisRoomAsync(command.RoomOwnerId);
        }
    }
}
