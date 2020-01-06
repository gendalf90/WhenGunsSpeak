using UnityEngine;
using Utils;

namespace Soldier
{
    public class Head : MonoBehaviour, ILookable, IFlippable
    {
        [SerializeField]
        private Vector2 lookingPoint;

        private SpriteRenderer[] renderers;

        private void Awake()
        {
            renderers = GetComponentsInChildren<SpriteRenderer>();
        }

        public void LookToPoint(Vector2 point)
        {
            var currentPosition = new Vector2(transform.position.x, transform.position.y);
            var currentAngle = currentPosition.GetAngle2(point);

            transform.rotation = Quaternion.Euler(0, 0, currentAngle);
        }

        public void FlipToRight()
        {
            foreach (var renderer in renderers)
            {
                renderer.flipY = false;
            }
        }

        public void FlipToLeft()
        {
            foreach(var renderer in renderers)
            {
                renderer.flipY = true;
            }
        }
    }
}
