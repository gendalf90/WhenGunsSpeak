﻿using UnityEngine;

namespace Shell
{
    public class Thrower : MonoBehaviour
    {
        [SerializeField]
        private float force;

        private Rigidbody2D rigidbody2d;
        private Transform fromTransform;
        private Transform toTransform;

        private void Awake()
        {
            rigidbody2d = GetComponent<Rigidbody2D>();
            fromTransform = transform.Find("From");
            toTransform = transform.Find("To");
        }

        private void Throw()
        {
            SetForce(force);
            AddForce();
        }

        private void SetForce(float force)
        {
            this.force = force;
        }

        private void AddForce()
        {
            rigidbody2d.AddForce(Speed, ForceMode2D.Impulse);
        }

        private Vector2 Speed
        {
            get
            {
                return Direction * force;
            }
        }

        private Vector2 Direction
        {
            get
            {
                return Vector3.Normalize(toTransform.position - fromTransform.position);
            }
        }
    }
}