using UnityEngine;

namespace Soldier
{
    public class Body : MonoBehaviour, IFlippable
    {
        private SpriteRenderer[] renderers;

        private void Awake()
        {
            renderers = GetComponentsInChildren<SpriteRenderer>();
        }

        public void FlipToRight()
        {
            foreach (var renderer in renderers)
            {
                renderer.flipX = false;
            }
        }

        public void FlipToLeft()
        {
            foreach (var renderer in renderers)
            {
                renderer.flipX = true;
            }
        }
    }
}
