using Messages;
using System;
using UnityEngine;

namespace Soldier
{
    class BodyRotation : MonoBehaviour
    {
        private Observable observable;
        private Transform handsTransform;
        private Transform shoulderTransform;

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
            handsTransform = transform.Find("Hands");
            shoulderTransform = transform.Find("Shoulder");
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

            Aim = e.LookingPosition;
            UpdateRotation();
        }

        public Guid PlayerGuid { get; set; }

        private Vector2 Aim { get; set; }

        private void UpdateRotation()
        {
            ResetHandsRotate();
            SetAngle();
            RotateHands();
        }

        private void SetAngle()
        {
            Angle = ShoulderPosition.GetAngle(Aim);
        }

        private void ResetHandsRotate()
        {
            handsTransform.RotateAround(ShoulderPosition, Vector3.forward, -Angle);
            handsTransform.rotation = Quaternion.identity;
        }

        private void RotateHands()
        {
            handsTransform.RotateAround(ShoulderPosition, Vector3.forward, Angle);
            handsTransform.rotation = Quaternion.Euler(0, 0, Angle);
        }

        private Vector2 ShoulderPosition
        {
            get
            {
                return shoulderTransform.position;
            }
        }

        private Vector2 HandsPosition
        {
            get
            {
                return handsTransform.position;
            }
        }

        private float Angle { get; set; }
    }
}