using Connection;
using Messages;
using System.Linq;
using UnityEngine;

namespace Server
{
    class UpdateRoomsHandler : MonoBehaviour
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
            observable.Subscribe<RefreshAllRoomsCommand>(RefreshAllRoomsHandler);
        }

        private async void RefreshAllRoomsHandler(RefreshAllRoomsCommand command)
        {
            var rooms = await roomConnection.GetAllRoomsAsync();
            var roomsShortData = rooms.Select(room => new RoomShortData(room.OwnerId, room.Header));
            observable.Publish(new AllRoomsAreUpdatedEvent(roomsShortData));
        }
    }
}
