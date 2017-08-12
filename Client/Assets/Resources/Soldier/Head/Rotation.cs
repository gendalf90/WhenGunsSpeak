using Messages;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soldier.Head
{
    class Rotation : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;

        private Vector2 lookPosition;

        private void Awake()
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        public Vector2 LookAt
        {
            get
            {
                return lookPosition;
            }
            set
            {
                lookPosition = value;
                RotationInDegrees = Position.GetAngle(lookPosition);
                SetLookDirection();
            }
        }

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

        private void SetLookDirection()
        {
            if (RotationInDegrees <= 90 || RotationInDegrees >= 270)
            {
                SetLookAtRight();
            }
            else
            {
                SetLookAtLeft();
            }
        }

        private void SetLookAtLeft()
        {
            spriteRenderer.flipY = true;
        }

        private void SetLookAtRight()
        {
            spriteRenderer.flipY = false;
        }
    }
}