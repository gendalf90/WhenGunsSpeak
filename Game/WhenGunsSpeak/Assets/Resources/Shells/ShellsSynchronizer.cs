using Messages;
using Server;
using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Shells
{
    class ShellsSynchronizer : MonoBehaviour
    {
        [SerializeField]
        private string typeMarker;

        private Observable observable;
        private HashSet<Guid> roomClientShellIds;
        private HashSet<Guid> roomOwnerShellIds;

        public ShellsSynchronizer()
        {
            roomClientShellIds = new HashSet<Guid>();
            roomOwnerShellIds = new HashSet<Guid>();
        }

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
        }

        private void OnEnable()
        {
            observable.Subscribe<AtRoomClientMessageReceivingEvent>(ReceiveIfIAmRoomClient);
            observable.Subscribe<AtRoomUpdatingEvent>(UpdateIfIAmRoomOwner);
        }

        private void OnDisable()
        {
            observable.Unsubscribe<AtRoomClientMessageReceivingEvent>(ReceiveIfIAmRoomClient);
            observable.Unsubscribe<AtRoomUpdatingEvent>(UpdateIfIAmRoomOwner);
        }

        private void ReceiveIfIAmRoomClient(AtRoomClientMessageReceivingEvent e)
        {
            var syncData = e.Data.DeserializeByMessagePack<ShellsSyncData>();

            if (syncData == null)
            {
                return;
            }

            if (syncData.TypeMarker != typeMarker)
            {
                return;
            }

            roomClientShellIds.SynchronizeWith(syncData.ShellIdsCreatedAtNow, WhenCreatedOnRoomClient, WhenDeletedOnRoomClient);
        }

        public void SetRoomOwnerIdsToSynchronization(IEnumerable<Guid> ids)
        {
            roomOwnerShellIds = new HashSet<Guid>(ids);
        }

        private void UpdateIfIAmRoomOwner(AtRoomUpdatingEvent e)
        {
            var syncData = new ShellsSyncData
            {
                TypeMarker = typeMarker,
                ShellIdsCreatedAtNow = roomOwnerShellIds
            };

            var syncDataBytes = syncData.SerializeByMessagePack();
            observable.Publish(new SendMessageCommand(syncDataBytes));
        }

        public event Action<Guid> WhenCreatedOnRoomClient;

        public event Action<Guid> WhenDeletedOnRoomClient;
    }
}
