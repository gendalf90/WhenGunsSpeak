using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soldier.Legs
{
    class Animation : MonoBehaviour
    {
        private Animator animator;
        private Transform legsTransform;

        private bool isRightMoving;
        private bool isLeftMoving;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            legsTransform = transform.FindChild("Legs");
        }

        private void Update()
        {
            UpdateAnimation();
        }

        private void UpdateAnimation()
        {
            var speed = GetSpeed();
            animator.SetBool("IsRun", speed != 0);
            animator.SetFloat("Speed", speed);
        }

        public bool IsRightMoving
        {
            get
            {
                return isRightMoving;
            }
            set
            {
                if(!isRightMoving && value)
                {
                    isLeftMoving = false;
                }

                isRightMoving = value;
            }
        }

        public bool IsLeftMoving
        {
            get
            {
                return isLeftMoving;
            }
            set
            {
                if(!isLeftMoving && value)
                {
                    isRightMoving = false;
                }

                isLeftMoving = value;
            }
        }

        private float GetSpeed()
        {
            float result = 0f;

            if (IsRightMoving)
            {
                result = 1f;
            }

            if (IsLeftMoving)
            {
                result = -1f;
            }

            if (IsFlipped)
            {
                result *= -1f;
            }

            return result;
        }

        private bool IsFlipped
        {
            get
            {
                return legsTransform.IsFlipX();
            }
        }
    }
}