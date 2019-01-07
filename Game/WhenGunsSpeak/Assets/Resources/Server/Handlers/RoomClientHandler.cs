using Messages;
using System;
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
            observable.Subscribe<SendMessageAtRoomClientCommand>(SendMessage);
        }

        private void OnDisable()
        {
            observable.Unsubscribe<IAmJoinedToRoomEvent>(IAmJoinedToRoomHandle);
            observable.Unsubscribe<MessageIsReceivedEvent>(ReceiveMessage);
            observable.Unsubscribe<SendMessageAtRoomClientCommand>(SendMessage);
        }

        private void IAmJoinedToRoomHandle(IAmJoinedToRoomEvent e)
        {
            MyId = e.MyId;
            AmIRoomClient = true;
        }

        private void ReceiveMessage(MessageIsReceivedEvent e)
        {
            if (AmIRoomClient)
            {
                observable.Publish(new AtRoomClientMessageReceivingEvent(e.Data, e.FromUserId));
            }
        }

        private void SendMessage(SendMessageAtRoomClientCommand command)
        {
            if (AmIRoomClient)
            {
                observable.Publish(new SendMessageCommand(command.Data));
            }
        }

        private void Update()
        {
            if (AmIRoomClient)
            {
                observable.Publish(new AtRoomClientUpdatingEvent(MyId));
            }
        }

        private bool AmIRoomClient { get; set; }

        private Guid MyId { get; set; }
    }
}
