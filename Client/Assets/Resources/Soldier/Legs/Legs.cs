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
            observable.Subscribe<AnimationCommand>(UpdateAnimationHandle);
            observable.Subscribe<LookEvent>(LookHandle);
        }

        private void OnDisable()
        {
            observable.Unsubscribe<AnimationCommand>(UpdateAnimationHandle);
            observable.Unsubscribe<LookEvent>(LookHandle);
        }

        private void LookHandle(LookEvent e)
        {
            if (e.Session != Session)
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

        private void UpdateAnimationHandle(AnimationCommand command)
        {
            if(command.Session != Session)
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

        public string Session { get; set; }
    }
}