using Messages;
using Soldier.Rotation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soldier.Legs
{
    class Legs : MonoBehaviour
    {
        private Observable observable;
        private Animation legsAnimation;
        private Flip flip;

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
            legsAnimation = GetComponent<Animation>();
            flip = GetComponent<Flip>();
        }

        private void OnEnable()
        {
            observable.Subscribe<StartAnimationCommand>(StartAnimationHandle);
            observable.Subscribe<LookEvent>(LookHandle);
        }

        private void OnDisable()
        {
            observable.Unsubscribe<StartAnimationCommand>(StartAnimationHandle);
            observable.Unsubscribe<LookEvent>(LookHandle);
        }

        private void LookHandle(LookEvent e)
        {
            if (e.Guid != Guid)
            {
                return;
            }

            if (e.Direction == LookDirection.Left)
            {
                flip.ToLeft();
            }

            if (e.Direction == LookDirection.Right)
            {
                flip.ToRight();
            }
        }

        private void StartAnimationHandle(StartAnimationCommand command)
        {
            if(command.Guid != Guid)
            {
                return;
            }

            if(command.Type == AnimationType.MoveLeft)
            {
                legsAnimation.IsLeftMoving = true;
            }

            if(command.Type == AnimationType.MoveRight)
            {
                legsAnimation.IsRightMoving = true;
            }

            if(command.Type == AnimationType.Stop)
            {
                legsAnimation.IsRightMoving = legsAnimation.IsLeftMoving = false;
            }
        }

        public Guid Guid { get; private set; }
    }
}