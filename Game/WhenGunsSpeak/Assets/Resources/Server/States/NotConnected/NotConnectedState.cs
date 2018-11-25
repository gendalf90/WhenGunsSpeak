using Messages;
using UnityEngine;

namespace Server
{
    class NotConnectedState : MonoBehaviour
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
            observable.Subscribe<ConnectToRoomsServiceCommand>(ConnectHandler);
            observable.Subscribe<OnConnectedToRoomsServiceEvent>(ConnectionIsReadyHandler);
        }

        private void UnsubscribeAll()
        {
            observable.Unsubscribe<ConnectToRoomsServiceCommand>(ConnectHandler);
            observable.Unsubscribe<OnConnectedToRoomsServiceEvent>(ConnectionIsReadyHandler);
        }

        private void ConnectHandler(ConnectToRoomsServiceCommand command)
        {
            observable.Publish(new StartRoomConnectionCommand());
        }

        private void ConnectionIsReadyHandler(OnConnectedToRoomsServiceEvent e)
        {
            StartConnectedState();
            Disable();
        }

        private void StartConnectedState()
        {
            var state = GetComponent<ConnectedState>();
            state.enabled = true;
        }

        private void Disable()
        {
            enabled = false;
        }
    }
}
