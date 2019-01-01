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
        private HashSet<Guid> shellIdsToSynchronization;

        public ShellsSynchronizer()
        {
            shellIdsToSynchronization = new HashSet<Guid>();
        }

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
        }

        private void OnEnable()
        {
            observable.Subscribe<WhenIAmRoomClientMessageReceivingEvent>(ReceiveIfIAmRoomClient);
        }

        private void OnDisable()
        {
            observable.Unsubscribe<WhenIAmRoomClientMessageReceivingEvent>(ReceiveIfIAmRoomClient);
        }

        private void ReceiveIfIAmRoomClient(WhenIAmRoomClientMessageReceivingEvent e)
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

            shellIdsToSynchronization.SynchronizeWith(syncData.ShellIdsCreatedAtNow, OnCreated, OnDeleted);
        }

        public void SendTheseShellIds(IEnumerable<Guid> ids)
        {
            var syncData = new ShellsSyncData
            {
                TypeMarker = typeMarker,
                ShellIdsCreatedAtNow = shellIdsToSynchronization
            };

            var syncDataBytes = syncData.SerializeByMessagePack();
            observable.Publish(new SendMessageCommand(syncDataBytes));
        }

        public event Action<Guid> OnCreated;

        public event Action<Guid> OnDeleted;
    }
}
