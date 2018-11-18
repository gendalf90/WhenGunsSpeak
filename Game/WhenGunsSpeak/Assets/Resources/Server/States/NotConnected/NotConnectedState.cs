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
            observable.Subscribe<ConnectCommand>(ConnectHandler);
            observable.Subscribe<OnConnectEvent>(ConnectionIsReadyHandler);
        }

        private void UnsubscribeAll()
        {
            observable.Unsubscribe<ConnectCommand>(ConnectHandler);
            observable.Unsubscribe<OnConnectEvent>(ConnectionIsReadyHandler);
        }

        private void ConnectHandler(ConnectCommand command)
        {
            observable.Publish(new StartRoomConnectionCommand());
        }

        private void ConnectionIsReadyHandler(OnConnectEvent e)
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
