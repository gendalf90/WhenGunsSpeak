using Messages;
using Soldier.Ground;
using System;
using UnityEngine;

namespace Soldier.Motion
{
    class Movement : MonoBehaviour
    {
        [SerializeField]
        private float moveForce = 0f;
        [SerializeField]
        private float jumpForce = 0f;

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
            observable.Subscribe<GroundEvent>(OnGroundedChange);
            observable.Subscribe<MoveCommand>(MoveHandler);
        }

        private void OnDisable()
        {
            observable.Unsubscribe<GroundEvent>(OnGroundedChange);
            observable.Unsubscribe<MoveCommand>(MoveHandler);
        }

        private void MoveHandler(MoveCommand command)
        {
            if(Session != command.Session)
            {
                return;
            }

            isRightMoving = command.IsToRight;
            isLeftMoving = command.IsToLeft;
            isJumping = command.IsJump;
        }

        private void OnGroundedChange(GroundEvent e)
        {
            if(e.Session == Session)
            {
                grounded = e.IsGrounded;
            }
        }

        private void FixedUpdate()
        {
            UpdateMovement();
            UpdateJumping();
            StayIfNeeded();
        }

        public string Session { get; set; }

        private void StayIfNeeded()
        {
            if (!isJumping && !isRightMoving && !isLeftMoving && grounded)
            {
                rigidbody2d.velocity = Vector2.zero;
            }
        }

        private void UpdateJumping()
        {
            isJumping = isJumping && grounded;

            if (isJumping)
            {
                rigidbody2d.AddForce(new Vector2(rigidbody2d.velocity.x / 3, jumpForce), ForceMode2D.Impulse);
            }
        }

        private void UpdateMovement()
        {
            if(isJumping || !grounded)
            {
                return;
            }

            var force = 0f;

            if(isRightMoving)
            {
                force += moveForce;
            }

            if(isLeftMoving)
            {
                force -= moveForce;
            }

            rigidbody2d.velocity = new Vector2(force, 0);
        }

        public void MoveRight()
        {
            isRightMoving = true;
        }

        public void StopRight()
        {
            isRightMoving = false;
        }
        
        public void MoveLeft()
        {
            isLeftMoving = true;
        }

        public void StopLeft()
        {
            isLeftMoving = false;
        }
        
        public void Jump()
        {
            isJumping = true;
        }
    }
}