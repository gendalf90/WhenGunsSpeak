using Messages;
using UnityEngine;

namespace Server
{
    class RoomOwnerHandler : MonoBehaviour
    {
        private Observable observable;

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
        }

        private void OnEnable()
        {
            observable.Subscribe<MyRoomIsStartedEvent>(MyRoomIsStartedHandle);
            observable.Subscribe<MessageIsReceivedEvent>(ReceiveMessage);
        }

        private void OnDisable()
        {
            observable.Unsubscribe<MyRoomIsStartedEvent>(MyRoomIsStartedHandle);
            observable.Unsubscribe<MessageIsReceivedEvent>(ReceiveMessage);
        }

        private void MyRoomIsStartedHandle(MyRoomIsStartedEvent e)
        {
            AmIRoomOwner = true;
        }

        private void ReceiveMessage(MessageIsReceivedEvent e)
        {
            if (AmIRoomOwner)
            {
                observable.Publish(new WhenIAmRoomOwnerMessageReceivingEvent(e.Data, e.FromUserId));
            }
        }

        private void Update()
        {
            if (AmIRoomOwner)
            {
                observable.Publish(new WhenIAmRoomOwnerUpdatingEvent());
            }
        }

        private bool AmIRoomOwner { get; set; }
    }
}
