using Messages;
using System;
using UnityEngine;

namespace Soldier
{
    class SoldierFlip : MonoBehaviour
    {
        private Observable observable;

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
        }

        private void OnEnable()
        {
            observable.Subscribe<SoldierLookingEvent>(LookingHandle);
        }

        private void OnDisable()
        {
            observable.Unsubscribe<SoldierLookingEvent>(LookingHandle);
        }

        private void LookingHandle(SoldierLookingEvent e)
        {
            if(e.PlayerGuid != PlayerGuid)
            {
                return;
            }

            SetLookPosition(e.LookingPosition);
            PublishFlipEvent();
        }

        private void SetLookPosition(Vector2 position)
        {
            LookingPosition = position;
        }

        public Guid PlayerGuid { get; set; }

        private void PublishFlipEvent()
        {
            observable.Publish(new SoldierFlipEvent(PlayerGuid, LookDirection));
        }

        private LookDirection LookDirection
        {
            get
            {
                if (Angle <= 90 || Angle >= 270)
                {
                    return LookDirection.Right;
                }
                else
                {
                    return LookDirection.Left;
                }
            }
        }

        private Vector2 LookingPosition { get; set; }

        private float Angle
        {
            get
            {
                return Position.GetAngle(LookingPosition);
            }
        }

        private Vector2 Position
        {
            get
            {
                return transform.position;
            }
        }
    }
}
