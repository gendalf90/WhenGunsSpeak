using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HeadRotation = Soldier.Head.Rotation;
using BodyRotation = Soldier.Body.Rotation;
using BodyFlip = Soldier.Body.Flip;
using HeadFlip = Soldier.Head.Flip;
using LegsFlip = Soldier.Legs.Flip;

namespace Soldier
{
    class Rotation : MonoBehaviour
    {
        private HeadRotation headRotation;
        private BodyRotation bodyRotation;
        private BodyFlip bodyFlip;
        private HeadFlip headFlip;
        private LegsFlip legsFlip;

        private Vector2 toPosition;

        private void Awake()
        {
            headRotation = GetComponentInChildren<HeadRotation>();
            bodyRotation = GetComponentInChildren<BodyRotation>();
            headFlip = GetComponentInChildren<HeadFlip>();
            bodyFlip = GetComponentInChildren<BodyFlip>();
            legsFlip = GetComponentInChildren<LegsFlip>();
        }

        public Vector2 ToPosition
        {
            get
            {
                return toPosition;
            }
            set
            {
                toPosition = value;
                TurnTheRest();
            }
        }

        private void TurnTheRest()
        {
            SetTurnPositions();
            FlipIfNeeded();
        }

        private void SetTurnPositions()
        {
            headRotation.LookAt = toPosition;
            bodyRotation.Aim = toPosition;
        }

        private void FlipIfNeeded()
        {
            if (Angle <= 90 || Angle >= 270)
            {
                LookAtRight();
            }
            else
            {
                LookAtLeft();
            }
        }

        private void LookAtRight()
        {
            headFlip.SetLookAtRight();
            bodyFlip.SetLookAtRight();
            legsFlip.ToRight();
        }

        private void LookAtLeft()
        {
            headFlip.SetLookAtLeft();
            bodyFlip.SetLookAtLeft();
            legsFlip.ToLeft();
        }

        private float Angle
        {
            get
            {
                return Position.GetAngle(toPosition);
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