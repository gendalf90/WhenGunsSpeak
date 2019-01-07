using Messages;
using Server;
using System;
using UnityEngine;
using Utils;

namespace Soldier
{
    class RoomSyncDataSender : MonoBehaviour
    {
        private Observable observable;
        private SoldierSyncData syncData;

        public RoomSyncDataSender()
        {
            syncData = new SoldierSyncData();
        }

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
        }

        private void OnEnable()
        {
            observable.Subscribe<SoldierLookingEvent>(HandleSoldierLookingEvent);
            observable.Subscribe<PositionEvent>(HandleSoldierPositionEvent);
            observable.Subscribe<LegsAnimationEvent>(HandleLegsAnimationEvent);
        }

        private void OnDisable()
        {
            observable.Unsubscribe<SoldierLookingEvent>(HandleSoldierLookingEvent);
            observable.Unsubscribe<PositionEvent>(HandleSoldierPositionEvent);
            observable.Unsubscribe<LegsAnimationEvent>(HandleLegsAnimationEvent);
        }

        private void HandleSoldierLookingEvent(SoldierLookingEvent e)
        {
            if(e.PlayerGuid == PlayerGuid)
            {
                syncData.LookingPosition = e.LookingPosition;
            }
        }

        private void HandleSoldierPositionEvent(PositionEvent e)
        {
            if(e.PlayerGuid == PlayerGuid)
            {
                syncData.Position = e.Position;
            }
        }

        private void HandleLegsAnimationEvent(LegsAnimationEvent e)
        {
            if (e.PlayerGuid == PlayerGuid)
            {
                syncData.LegsAnimationType = e.Type;
            }
        }

        private void Update()
        {
            var inputDataBytes = syncData.SerializeByMessagePack();
            observable.Publish(new SendMessageCommand(inputDataBytes));
        }

        public Guid PlayerGuid
        {
            get
            {
                return syncData.PlayerGuid;
            }
            set
            {
                syncData.PlayerGuid = value;
            }
        }
    }
}
