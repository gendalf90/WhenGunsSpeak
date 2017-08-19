using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soldier
{
    class Motion : MonoBehaviour
    {
        [SerializeField]
        private float moveForce = 0f;
        [SerializeField]
        private float jumpForce = 0f;

        private Ground ground;
        private Rigidbody2D rigidbody2d;

        private bool isRightMoving;
        private bool isLeftMoving;
        private bool isJumping;

        private void Awake()
        {
            ground = GetComponentInChildren<Ground>();
            rigidbody2d = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            UpdateMovement();
            UpdateJumping();
        }

        private void UpdateJumping()
        {
            if (!isJumping)
            {
                return;
            }

            isJumping = false;

            if(!ground.IsIntersect)
            {
                return;
            }

            var movingForce = 0f;

            if(isRightMoving)
            {
                movingForce += moveForce;
            }

            if (isLeftMoving)
            {
                movingForce -= moveForce;
            }

            rigidbody2d.AddForce(new Vector2(movingForce, jumpForce), ForceMode2D.Impulse);
        }

        private void UpdateMovement()
        {
            if(isJumping || !isRightMoving && !isLeftMoving || !ground.IsIntersect)
            {
                return;
            }

            var movement = Position;

            if(isRightMoving)
            {
                movement += RightMovement;
            }

            if(isLeftMoving)
            {
                movement += LeftMovement;
            }

            rigidbody2d.MovePosition(movement);
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

        private Vector2 RightMovement
        {
            get
            {
                return Vector2.right * moveForce * Time.deltaTime;
            }
        }

        private Vector2 LeftMovement
        {
            get
            {
                return Vector2.left * moveForce * Time.deltaTime;
            }
        }
        
        public void Jump()
        {
            isJumping = true;
        }

        private Vector2 Position
        {
            get
            {
                return rigidbody2d.position;
            }
        }
    }
}