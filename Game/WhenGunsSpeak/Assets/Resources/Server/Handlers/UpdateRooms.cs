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

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
        }

        public void Start(IRoomConnection connection)
        {
            roomConnection = connection;
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
