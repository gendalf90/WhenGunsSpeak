using Configuration;
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
        private Parameters parameters;

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
            parameters = FindObjectOfType<Parameters>();
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
            observable.Subscribe<RefreshAllRoomsCommand>(RefreshAllRoomsHandler);
        }

        private void UnsubscribeAll()
        {
            observable.Unsubscribe<RefreshAllRoomsCommand>(RefreshAllRoomsHandler);
        }

        private async void RefreshAllRoomsHandler(RefreshAllRoomsCommand command)
        {
            var rooms = await roomConnection.GetAllRoomsAsync();
            var roomsShortData = rooms.Select(room => new RoomShortData(room.OwnerId, room.Header));
            observable.Publish(new AllRoomsAreUpdatedEvent(roomsShortData));
        }

        public void Start(IRoomConnection roomConnection)
        {
            this.roomConnection = roomConnection;
            enabled = true;
        }
    }
}
