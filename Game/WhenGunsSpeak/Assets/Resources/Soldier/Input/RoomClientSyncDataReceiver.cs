using Messages;
using Server;
using System;
using UnityEngine;
using Utils;

namespace Soldier
{
    class RoomClientSyncDataReceiver : MonoBehaviour
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
            var message = e.Data.DeserializeByMessagePack<SoldierSyncData>();

            if (message == null)
            {
                return;
            }

            if (message.PlayerGuid != PlayerGuid)
            {
                return;
            }

            observable.Publish(new SetSoldierPositionCommand(PlayerGuid, message.Position));
            observable.Publish(new SoldierLookingEvent(PlayerGuid, message.LookingPosition));
            observable.Publish(new StartLegsAnimationCommand(PlayerGuid, message.LegsAnimationType));
        }

        public Guid PlayerGuid { get; set; }
    }
}
