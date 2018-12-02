using Connection;
using Messages;
using UnityEngine;

namespace Server
{
    class StartNewRoomHandler : MonoBehaviour
    {
        private IRoomConnection roomConnection;
        private Observable observable;

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
        }

        public void Start(IRoomConnection connection)
        {
            roomConnection = connection;
            observable.Subscribe<StartNewRoomCommand>(StartHandle);
        }

        private async void StartHandle(StartNewRoomCommand command)
        {
            await roomConnection.CreateMyRoomAsync(command.Header);
            observable.Publish(new NewRoomIsStartedEvent());
        }
    }
}
