using Connection;
using Messages;
using System.Threading;
using UnityEngine;
using Utils;

namespace Server
{
    class UserRoomJoiningAttemptHandler : MonoBehaviour
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
            roomConnection.Subscribe<RoomJoiningData>(next: UserJoiningHandle);
            observable.Subscribe<JoinUserToRoomCommand>(JoinUserToRoomHandle);
            observable.Subscribe<DenyUserRoomJoiningCommand>(DenyUserRoomJoiningHandle);
        }

        private void UserJoiningHandle(RoomJoiningData value)
        {
            synchronization.Post(state =>
            {
                observable.Publish(new UserIsJoiningToRoomEvent(value.UserId));
            }, value);
        }

        private async void JoinUserToRoomHandle(JoinUserToRoomCommand command)
        {
            await roomConnection.JoinTheUserToMyRoomAsync(command.UserId);
        }

        private async void DenyUserRoomJoiningHandle(DenyUserRoomJoiningCommand command)
        {
            await roomConnection.DenyToUserToMyRoomJoiningAsync(command.UserId, command.Message);
        }
    }
}
