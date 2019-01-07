using Messages;
using System;
using UnityEngine;

namespace Soldier
{
    class LegsFlip : MonoBehaviour
    {
        private Observable observable;

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
        }

        private void OnEnable()
        {
            observable.Subscribe<SoldierFlipEvent>(FlipHandle);
        }

        private void OnDisable()
        {
            observable.Unsubscribe<SoldierFlipEvent>(FlipHandle);
        }

        public Guid PlayerGuid { get; set; }

        private void FlipHandle(SoldierFlipEvent e)
        {
            if (e.PlayerGuid != PlayerGuid)
            {
                return;
            }

            if (e.LookDirection == LookDirection.Left)
            {
                ToLeft();
            }

            if (e.LookDirection == LookDirection.Right)
            {
                ToRight();
            }
        }

        private void ToLeft()
        {
            transform.SetFlipX(true);
        }

        private void ToRight()
        {
            transform.SetFlipX(false);
        }
    }
}