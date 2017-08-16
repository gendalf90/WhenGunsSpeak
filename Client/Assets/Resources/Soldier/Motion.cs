using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soldier
{
    public class Motion : MonoBehaviour
    {
        [SerializeField]
        private float moveForce = 0f;
        [SerializeField]
        private float jumpForce = 0f;

        private Rigidbody2D rigidbody2d;
        private ConstantForce2D motionForce;

        private void Awake()
        {
            rigidbody2d = GetComponent<Rigidbody2D>();
        }

        public void MoveRight()
        {
            //rigidbody2d.MovePosition(transform.position + destination * Time.deltaTime); //как в shell'ах
        }

        public void MoveLeft()
        {

        }

        public void Jump()
        {
            rigidbody2d.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
    }
}