using Messages;
using System;
using UnityEngine;

namespace Soldier
{
    class LegsAnimation : MonoBehaviour
    {
        private Animator animator;
        private Observable observable;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            observable = FindObjectOfType<Observable>();
        }

        private void OnEnable()
        {
            observable.Subscribe<SoldierMovingEvent>(HandleMovingEvent);
            observable.Subscribe<StartLegsAnimationCommand>(HandleStartAnimationCommand);
        }

        private void OnDisable()
        {
            observable.Unsubscribe<SoldierMovingEvent>(HandleMovingEvent);
            observable.Unsubscribe<StartLegsAnimationCommand>(HandleStartAnimationCommand);
        }

        public Guid PlayerGuid { get; set; }

        private void HandleMovingEvent(SoldierMovingEvent e)
        {
            if (e.PlayerGuid != PlayerGuid)
            {
                return;
            }

            IsRightMoving = e.IsRightMoving;
            IsLeftMoving = e.IsLeftMoving;

            if(IsMultidirectionalMoving)
            {
                StopMoving();
            }
        }

        private void HandleStartAnimationCommand(StartLegsAnimationCommand command)
        {
            if (command.PlayerGuid != PlayerGuid)
            {
                return;
            }

            IsRightMoving = command.Type == LegsAnimationType.MoveRight;
            IsLeftMoving = command.Type == LegsAnimationType.MoveLeft;
        }

        private void PublishAnimationEvent()
        {
            var currentAnimationType = LegsAnimationType.Stop;

            if(IsRightMoving)
            {
                currentAnimationType = LegsAnimationType.MoveRight;
            }

            if(IsLeftMoving)
            {
                currentAnimationType = LegsAnimationType.MoveLeft;
            }

            observable.Publish(new LegsAnimationEvent(PlayerGuid, currentAnimationType));
        }

        private void Update()
        {
            UpdateAnimation();
            PublishAnimationEvent();
        }

        private void StopMoving()
        {
            IsLeftMoving = IsRightMoving = false;
        }

        private void UpdateAnimation()
        {
            var speed = GetSpeed();
            animator.SetBool("IsRun", speed != 0);
            animator.SetFloat("Speed", speed);
        }

        private bool IsRightMoving { get; set; }

        private bool IsLeftMoving { get; set; }

        private bool IsMultidirectionalMoving
        {
            get
            {
                return IsRightMoving && IsLeftMoving;
            }
        }

        private float GetSpeed()
        {
            float result = 0f;

            if (IsRightMoving)
            {
                result = 1f;
            }

            if (IsLeftMoving)
            {
                result = -1f;
            }

            if (IsFlipped)
            {
                result *= -1f;
            }

            return result;
        }

        private bool IsFlipped
        {
            get
            {
                return transform.IsFlipX();
            }
        }
    }
}