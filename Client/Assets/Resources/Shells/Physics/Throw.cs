using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shells
{
    public class Throw : MonoBehaviour
    {
        private Rigidbody2D rigidbody2d;
        private Transform fromTransform;
        private Transform toTransform;

        private float force;

        private void Awake()
        {
            rigidbody2d = GetComponent<Rigidbody2D>();
            fromTransform = transform.FindChild("From");
            toTransform = transform.FindChild("To");
        }

        public void Begin(float force)
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