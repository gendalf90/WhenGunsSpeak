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

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
        }

        private void Start()
        {
            synchronization = SynchronizationContext.Current;
        }

        public void Start(IRoomConnection connection)
        {
            roomConnection = connection;
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
