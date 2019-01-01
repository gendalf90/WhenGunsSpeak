using Messages;
using UnityEngine;

namespace Server
{
    class RoomClientHandler : MonoBehaviour
    {
        private Observable observable;

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
        }

        private void OnEnable()
        {
            observable.Subscribe<IAmJoinedToRoomEvent>(IAmJoinedToRoomHandle);
            observable.Subscribe<MessageIsReceivedEvent>(ReceiveMessage);
        }

        private void OnDisable()
        {
            observable.Unsubscribe<IAmJoinedToRoomEvent>(IAmJoinedToRoomHandle);
            observable.Unsubscribe<MessageIsReceivedEvent>(ReceiveMessage);
        }

        private void IAmJoinedToRoomHandle(IAmJoinedToRoomEvent e)
        {
            AmIRoomClient = true;
        }

        private void ReceiveMessage(MessageIsReceivedEvent e)
        {
            if (AmIRoomClient)
            {
                observable.Publish(new WhenIAmRoomClientMessageReceivingEvent(e.Data, e.FromUserId));
            }
        }

        private void Update()
        {
            if (AmIRoomClient)
            {
                observable.Publish(new WhenIAmRoomClientUpdatingEvent());
            }
        }

        private bool AmIRoomClient { get; set; }
    }
}
