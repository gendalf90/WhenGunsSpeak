using Input;
using Messages;
using System;
using UnityEngine;

namespace Soldier
{
    class RoomPlayer : MonoBehaviour
    {
        private Observable observable;

        private bool isRightMoving;
        private bool isLeftMoving;
        private bool isJumping;

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
        }

        private void OnEnable()
        {
            observable.Subscribe<CursorEvent>(CursorHandle);
            observable.Subscribe<StartRightEvent>(StartRightHandle);
            observable.Subscribe<StopRightEvent>(StopRightHandle);
            observable.Subscribe<StartLeftEvent>(StartLeftHandle);
            observable.Subscribe<StopLeftEvent>(StopLeftHandle);
            observable.Subscribe<StartJumpEvent>(StartJumpHandle);
            observable.Subscribe<StopJumpEvent>(StopJumpHandle);
        }

        private void OnDisable()
        {
            observable.Unsubscribe<CursorEvent>(CursorHandle);
            observable.Unsubscribe<StartRightEvent>(StartRightHandle);
            observable.Unsubscribe<StopRightEvent>(StopRightHandle);
            observable.Unsubscribe<StartLeftEvent>(StartLeftHandle);
            observable.Unsubscribe<StopLeftEvent>(StopLeftHandle);
            observable.Unsubscribe<StartJumpEvent>(StartJumpHandle);
            observable.Unsubscribe<StopJumpEvent>(StopJumpHandle);
        }

        private void Update()
        {
            observable.Publish(new SoldierMovingEvent(PlayerGuid, isRightMoving, isLeftMoving, isJumping));
        }

        private void CursorHandle(CursorEvent e)
        {
            observable.Publish(new SoldierLookingEvent(PlayerGuid, e.WorldPosition));
        }

        private void StartRightHandle(StartRightEvent e)
        {
            isRightMoving = true;
        }

        private void StopRightHandle(StopRightEvent e)
        {
            isRightMoving = false;
        }

        private void StartLeftHandle(StartLeftEvent e)
        {
            isLeftMoving = true;
        }

        private void StopLeftHandle(StopLeftEvent e)
        {
            isLeftMoving = false;
        }

        private void StartJumpHandle(StartJumpEvent e)
        {
            isJumping = true;
        }

        private void StopJumpHandle(StopJumpEvent e)
        {
            isJumping = false;
        }

        public Guid PlayerGuid { get; set; }
    }
}
