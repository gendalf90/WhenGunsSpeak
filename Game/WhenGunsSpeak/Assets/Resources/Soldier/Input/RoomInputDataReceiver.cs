using Messages;
using Server;
using System;
using UnityEngine;
using Utils;

namespace Soldier
{
    class RoomInputDataReceiver : MonoBehaviour
    {
        private Observable observable;

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
        }

        private void OnEnable()
        {
            observable.Subscribe<MessageIsReceivedEvent>(ReceiveMessage);
        }

        private void OnDisable()
        {
            observable.Unsubscribe<MessageIsReceivedEvent>(ReceiveMessage);
        }

        private void ReceiveMessage(MessageIsReceivedEvent e)
        {
            var message = e.Data.DeserializeByMessagePack<PlayerInputData>();

            if(message == null)
            {
                return;
            }

            if(message.PlayerGuid != PlayerGuid)
            {
                return;
            }

            observable.Publish(new SoldierMovingEvent(PlayerGuid, message.IsRightMoving, message.IsLeftMoving, message.IsJumping));
            observable.Publish(new SoldierLookingEvent(PlayerGuid, message.LookingPosition));
        }

        public Guid PlayerGuid { get; set; }
    }
}
