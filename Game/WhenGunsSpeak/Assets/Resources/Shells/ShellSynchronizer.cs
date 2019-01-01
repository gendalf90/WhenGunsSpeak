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
        private Rigidbody2D rigidbody2d;

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
            rigidbody2d = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            observable.Subscribe<MessageIsReceivedEvent>(ReceiveMessage);
        }

        private void OnDisable()
        {
            observable.Unsubscribe<MessageIsReceivedEvent>(ReceiveMessage);
        }

        public Guid ShellGuid { get; set; }

        private void ReceiveMessage(MessageIsReceivedEvent e)
        {
            if(!IsKinematic)
            {
                return;
            }

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

        private void Update()
        {
            if (IsKinematic)
            {
                return;
            }

            var syncData = new ShellSyncData
            {
                Guid = ShellGuid,
                Position = Position,
                Rotation = Rotation
            };

            var syncDataBytes = syncData.SerializeByMessagePack();
            observable.Publish(new SendMessageCommand(syncDataBytes));
        }

        private bool IsKinematic => rigidbody2d.isKinematic;

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
