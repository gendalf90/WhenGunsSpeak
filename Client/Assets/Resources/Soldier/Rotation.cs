using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HeadRotation = Soldier.Head.Rotation;
using BodyRotation = Soldier.Body.Rotation;
using BodyFlip = Soldier.Body.Flip;
using HeadFlip = Soldier.Head.Flip;

namespace Soldier
{
    class Rotation : MonoBehaviour
    {
        private HeadRotation headRotation;
        private BodyRotation bodyRotation;
        private BodyFlip bodyFlip;
        private HeadFlip headFlip;

        private Vector2 toPosition;

        private void Awake()
        {
            headRotation = GetComponentInChildren<HeadRotation>();
            bodyRotation = GetComponentInChildren<BodyRotation>();
            headFlip = GetComponentInChildren<HeadFlip>();
            bodyFlip = GetComponentInChildren<BodyFlip>();
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
                headFlip.SetLookAtRight();
                bodyFlip.SetLookAtRight();
            }
            else
            {
                headFlip.SetLookAtLeft();
                bodyFlip.SetLookAtLeft();
            }
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