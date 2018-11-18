using Messages;
using UnityEngine;

namespace Server
{
    class ConnectedState : MonoBehaviour
    {
        private Observable observable;

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
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
            observable.Subscribe<StartNewRoomCommand>(StartNewRoomHandler);
            observable.Subscribe<OnNewRoomHasStartedEvent>(OnNewRoomHasStartedHandler);
        }

        private void UnsubscribeAll()
        {
            observable.Unsubscribe<RefreshAllRoomsCommand>(RefreshAllRoomsHandler);
            observable.Unsubscribe<StartNewRoomCommand>(StartNewRoomHandler);
            observable.Unsubscribe<OnNewRoomHasStartedEvent>(OnNewRoomHasStartedHandler);
        }

        private void RefreshAllRoomsHandler(RefreshAllRoomsCommand command)
        {
            observable.Publish(new StartRoomsUpdatingCommand());
        }

        private void StartNewRoomHandler(StartNewRoomCommand command)
        {
            observable.Publish(new StartRoomCommand(command.Header));
        }

        private void OnNewRoomHasStartedHandler(OnNewRoomHasStartedEvent e)
        {
            StartRoomOwnerState();
            Disable();
        }

        private void StartRoomOwnerState()
        {
            var state = GetComponent<RoomOwnerState>();
            state.enabled = true;
        }

        private void Disable()
        {
            enabled = false;
        }
    }
}
