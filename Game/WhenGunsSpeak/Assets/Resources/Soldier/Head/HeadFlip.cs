using Messages;
using System;
using UnityEngine;

namespace Soldier
{
    class HeadFlip : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;
        private Observable observable;

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
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
            if(e.PlayerGuid != PlayerGuid)
            {
                return;
            }

            if (e.LookDirection == LookDirection.Left)
            {
                SetLookAtLeft();
            }

            if (e.LookDirection == LookDirection.Right)
            {
                SetLookAtRight();
            }
        }

        public void SetLookAtLeft()
        {
            spriteRenderer.flipY = true;
        }

        public void SetLookAtRight()
        {
            spriteRenderer.flipY = false;
        }
    }
}