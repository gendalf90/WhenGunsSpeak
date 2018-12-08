using Connection;
using Messages;
using UnityEngine;

namespace Server
{
    class StartNewRoomHandler : MonoBehaviour
    {
        private IRoomConnection roomConnection;
        private Observable observable;
        private StartRoomConnectionHandler roomsConnectionHandler;

        private void Awake()
        {
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
            observable.Subscribe<StartNewRoomCommand>(StartHandle);
        }

        private async void StartHandle(StartNewRoomCommand command)
        {
            await roomConnection.CreateMyRoomAsync(command.Header);
            observable.Publish(new NewRoomIsStartedEvent());
        }
    }
}
