using Messages;
using System;
using UnityEngine;

namespace Soldier
{
    class HeadRotation : MonoBehaviour
    {
        private Observable observable;
        private Vector2 lookPosition;

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
            if (e.PlayerGuid == PlayerGuid)
            {
                RotationInDegrees = Position.GetAngle(e.LookingPosition);
            }
        }

        public Guid PlayerGuid { get; set; }

        private Vector2 Position
        {
            get
            {
                return transform.position;
            }
        }

        private float RotationInDegrees
        {
            get
            {
                return transform.rotation.eulerAngles.z;
            }
            set
            {
                transform.rotation = Quaternion.Euler(0, 0, value);
            }
        }
    }
}