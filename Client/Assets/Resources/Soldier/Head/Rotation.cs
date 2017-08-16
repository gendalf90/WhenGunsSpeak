using Messages;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soldier.Head
{
    class Rotation : MonoBehaviour
    {
        private Vector2 lookPosition;

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
    }
}