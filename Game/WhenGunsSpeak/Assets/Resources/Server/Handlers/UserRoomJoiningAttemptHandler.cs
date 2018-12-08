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
