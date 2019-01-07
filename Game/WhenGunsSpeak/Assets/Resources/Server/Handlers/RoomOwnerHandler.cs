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
            observable.Subscribe<SendMessageAtRoomCommand>(SendMessage);
        }

        private void OnDisable()
        {
            observable.Unsubscribe<MyRoomIsStartedEvent>(MyRoomIsStartedHandle);
            observable.Unsubscribe<MessageIsReceivedEvent>(ReceiveMessage);
            observable.Unsubscribe<SendMessageAtRoomCommand>(SendMessage);
        }

        private void MyRoomIsStartedHandle(MyRoomIsStartedEvent e)
        {
            AmIRoomOwner = true;
        }

        private void ReceiveMessage(MessageIsReceivedEvent e)
        {
            if (AmIRoomOwner)
            {
                observable.Publish(new AtRoomMessageReceivingEvent(e.Data, e.FromUserId));
            }
        }

        private void SendMessage(SendMessageAtRoomCommand command)
        {
            if (AmIRoomOwner)
            {
                observable.Publish(new SendMessageCommand(command.Data));
            }
        }

        private void Update()
        {
            if (AmIRoomOwner)
            {
                observable.Publish(new AtRoomUpdatingEvent());
            }
        }

        private bool AmIRoomOwner { get; set; }
    }
}
