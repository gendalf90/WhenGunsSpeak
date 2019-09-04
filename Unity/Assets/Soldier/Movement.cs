using UnityEngine;

namespace Soldier
{
    public class Movement : MonoBehaviour
    {
        [SerializeField]
        private float movingForce = 0f;

        private Rigidbody2D rigidbody2d;

        private void Awake()
        {
            rigidbody2d = GetComponent<Rigidbody2D>();
        }

        public void MoveToPoint(Vector2 destination)
        {
            var relativeOfCurrentPosition = destination - rigidbody2d.position;

            rigidbody2d.velocity = relativeOfCurrentPosition.normalized * movingForce;
        }

        public void StopMoving()
        {
            rigidbody2d.velocity = Vector2.zero;
        }
    }
}
