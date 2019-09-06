using UnityEngine;

namespace Soldier
{
    public class Legs : MonoBehaviour, IFlippable
    {
        private SpriteRenderer[] renderers;

        private void Awake()
        {
            renderers = GetComponentsInChildren<SpriteRenderer>();
        }

        public void FlipToLeft()
        {
            foreach (var renderer in renderers)
            {
                renderer.flipX = true;
            }
        }

        public void FlipToRight()
        {
            foreach (var renderer in renderers)
            {
                renderer.flipX = false;
            }
        }
    }
}
