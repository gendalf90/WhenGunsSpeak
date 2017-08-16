using Messages;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soldier.Body
{
    class Rotation : MonoBehaviour
    {
        private Transform handsTransform;
        private Transform shoulderTransform;

        private Vector2 aimPosition;
        private float currentAngle;

        private void Awake()
        {
            handsTransform = transform.FindChild("Hands");
            shoulderTransform = transform.FindChild("Shoulder");
        }

        public Vector2 Aim
        {
            get
            {
                return aimPosition;
            }
            set
            {
                aimPosition = value;
                UpdateRotation();
            }
        }

        private void UpdateRotation()
        {
            ResetHandsRotate();
            SetAngle();
            RotateHands();
        }

        private void SetAngle()
        {
            currentAngle = ShoulderPosition.GetAngle(aimPosition);
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

        private float Angle
        {
            get
            {
                return currentAngle;
            }
        }
    }
}