using Messages;
using System;
using UnityEngine;

namespace Soldier
{
    class SoldierMovement : MonoBehaviour
    {
        [SerializeField]
        private float movingForce = 0f;

        [SerializeField]
        private float jumpingForce = 0f;

        private Observable observable;
        private Rigidbody2D rigidbody2d;

        private bool isRightMoving;
        private bool isLeftMoving;
        private bool isJumping;
        private bool grounded;

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
            rigidbody2d = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            observable.Subscribe<GroundingEvent>(HandleGroundingEvent);
            observable.Subscribe<SoldierMovingEvent>(HandleMovingEvent);
        }

        private void OnDisable()
        {
            observable.Unsubscribe<GroundingEvent>(HandleGroundingEvent);
            observable.Unsubscribe<SoldierMovingEvent>(HandleMovingEvent);
        }

        public Guid PlayerGuid { get; set; }

        private void HandleMovingEvent(SoldierMovingEvent e)
        {
            if(e.PlayerGuid != PlayerGuid)
            {
                return;
            }

            isRightMoving = e.IsRightMoving;
            isLeftMoving = e.IsLeftMoving;
            isJumping = e.IsJumping;
        }

        private void HandleGroundingEvent(GroundingEvent e)
        {
            if(e.PlayerGuid == PlayerGuid)
            {
                grounded = e.IsGrounded;
            }
        }

        private void FixedUpdate()
        {
            UpdateMovement();
        }

        private void Update()
        {
            UpdateJumping();
        }

        private void UpdateJumping()
        {
            if (isJumping && grounded)
            {
                rigidbody2d.AddForce(new Vector2(0, jumpingForce));
            }
        }

        private void UpdateMovement()
        {
            var force = 0f;

            if(isRightMoving)
            {
                force += movingForce;
            }

            if(isLeftMoving)
            {
                force -= movingForce;
            }

            rigidbody2d.velocity = new Vector2(force, rigidbody2d.velocity.y);
        }
    }
}