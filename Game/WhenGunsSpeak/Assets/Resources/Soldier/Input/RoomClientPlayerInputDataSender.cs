using Input;
using Messages;
using Server;
using System;
using UnityEngine;
using Utils;

namespace Soldier
{
    class RoomClientPlayerInputDataSender : MonoBehaviour
    {
        private Observable observable;

        private PlayerInputData inputData;

        public RoomClientPlayerInputDataSender()
        {
            inputData = new PlayerInputData();
        }

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
            var inputDataBytes = inputData.SerializeByMessagePack();
            observable.Publish(new SendMessageCommand(inputDataBytes));
        }

        private void CursorHandle(CursorEvent e)
        {
            inputData.LookingPosition = e.WorldPosition;
        }

        private void StartRightHandle(StartRightEvent e)
        {
            inputData.IsRightMoving = true;
        }

        private void StopRightHandle(StopRightEvent e)
        {
            inputData.IsRightMoving = false;
        }

        private void StartLeftHandle(StartLeftEvent e)
        {
            inputData.IsLeftMoving = true;
        }

        private void StopLeftHandle(StopLeftEvent e)
        {
            inputData.IsLeftMoving = false;
        }

        private void StartJumpHandle(StartJumpEvent e)
        {
            inputData.IsJumping = true;
        }

        private void StopJumpHandle(StopJumpEvent e)
        {
            inputData.IsJumping = false;
        }

        public Guid PlayerGuid
        {
            get
            {
                return inputData.PlayerGuid;
            }
            set
            {
                inputData.PlayerGuid = value;
            }
        }
    }
}
