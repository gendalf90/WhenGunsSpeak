using System.Linq;
using UnityEngine;

namespace Soldier
{
    class GroundCheckScript : MonoBehaviour
    {
        private CircleCollider2D circleCollider;

        private void Awake()
        {
            circleCollider = GetComponent<CircleCollider2D>();
        }

        public bool IsGrounded { get; set; }

        private int GroundMask
        {
            get
            {
                return 1 << 8;
            }
        }

        private void Update()
        {
            IsGrounded = Physics2D.OverlapCircle(transform.position, circleCollider.radius, GroundMask) != null;
        }
    }
}