using Messages;
using Server;
using System;
using UnityEngine;
using Utils;

namespace Shells
{
    class ShellSynchronizer : MonoBehaviour
    {
        private Observable observable;

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
        }

        private void OnEnable()
        {
            observable.Subscribe<AtRoomClientMessageReceivingEvent>(ReceiveMessage);
            observable.Subscribe<AtRoomUpdatingEvent>(SendMessage);
        }

        private void OnDisable()
        {
            observable.Unsubscribe<AtRoomClientMessageReceivingEvent>(ReceiveMessage);
            observable.Unsubscribe<AtRoomUpdatingEvent>(SendMessage);
        }

        public Guid ShellGuid { get; set; }

        private void ReceiveMessage(AtRoomClientMessageReceivingEvent e)
        {
            var syncData = e.Data.DeserializeByMessagePack<ShellSyncData>();

            if (syncData == null)
            {
                return;
            }

            if (syncData.Guid != ShellGuid)
            {
                return;
            }

            Position = syncData.Position;
            Rotation = syncData.Rotation;
        }

        private void SendMessage(AtRoomUpdatingEvent e)
        {
            var syncData = new ShellSyncData
            {
                Guid = ShellGuid,
                Position = Position,
                Rotation = Rotation
            };

            var syncDataBytes = syncData.SerializeByMessagePack();
            observable.Publish(new SendMessageCommand(syncDataBytes));
        }

        private Vector2 Position
        {
            get
            {
                return transform.position;
            }
            set
            {
                transform.position = value;
            }
        }

        private float Rotation
        {
            get
            {
                return transform.rotation.z;
            }
            set
            {
                transform.rotation = Quaternion.Euler(0, 0, value);
            }
        }
    }
}
