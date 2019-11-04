using UnityEngine;

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

        private void Start()
        {
            Throw();
        }

        private void Throw()
        {
            rigidbody2d.AddForce(Speed, ForceMode2D.Impulse);
        }

        public void SetForce(float force)
        {
            this.force = force;
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