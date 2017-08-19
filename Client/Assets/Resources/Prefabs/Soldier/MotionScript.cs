//using UnityEngine;

//namespace Soldier
//{
//    class MotionScript : MonoBehaviour
//    {
//        [SerializeField]
//        private float moveForce = 0f;
//        [SerializeField]
//        private float jumpForce = 0f;
//        [SerializeField]
//        private float flightForce = 0f;
//        [SerializeField]
//        private float jumpIntervalInSeconds = 0f;

//        private Rigidbody2D rigidbody2d;
//        private ConstantForce2D motionForce;
//        private GroundCheckScript groundedCheck;
//        private ControlScript control;

//        private float startJumpTime;

//        private void Awake()
//        {
//            rigidbody2d = GetComponent<Rigidbody2D>();
//            motionForce = GetComponent<ConstantForce2D>();
//            groundedCheck = GetComponentInChildren<GroundCheckScript>();
//            control = GetComponent<ControlScript>();
//        }

//        private bool IsJumpPossible
//        {
//            get
//            {
//                return Time.realtimeSinceStartup - startJumpTime > jumpIntervalInSeconds;
//            }
//        }

//        private void UpdateStartJumpTime()
//        {
//            startJumpTime = Time.realtimeSinceStartup;
//        }

//        private void JumpIfNeeded()
//        {
//            if(control.IsJump && groundedCheck.IsGrounded && IsJumpPossible)
//            {
//                CreateJump();
//                UpdateStartJumpTime();
//            }
//        }

//        private void CreateJump()
//        {
//            rigidbody2d.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
//        }

//        private void Update()
//        {
//            UpdateMotion();
//        }

//        private void UpdateMotion()
//        {
//            float moveForce = GetMoveForce();

//            if (moveForce != 0)
//            {
//                rigidbody2d.velocity = new Vector2(moveForce, rigidbody2d.velocity.y);
//            }

//            float flightForce = GetFlightForce();
//            motionForce.force = new Vector2(0f, flightForce);

//            JumpIfNeeded();
//        }

//        private float GetMoveForce()
//        {
//            float moveDirectionCoefficient = GetMoveDirectionCoefficient();
//            float moveTypeCoefficient = GetMoveTypeCoefficient();
//            return moveForce * moveDirectionCoefficient * moveTypeCoefficient;
//        }

//        private float GetMoveDirectionCoefficient()
//        {
//            if(control.IsMoveRight && !control.IsMoveLeft)
//            {
//                return 1f;
//            }

//            if(control.IsMoveLeft && !control.IsMoveRight)
//            {
//                return -1f;
//            }

//            return 0f;
//        }

//        private float GetMoveTypeCoefficient()
//        {
//            if(control.IsFlight)
//            {
//                return 0.5f;
//            }
//            else if (groundedCheck.IsGrounded)
//            {
//                return 1f;
//            }

//            return 0f;
//        }

//        private float GetFlightForce()
//        {
//            return control.IsFlight ? flightForce : 0f;
//        }
//    }
//}