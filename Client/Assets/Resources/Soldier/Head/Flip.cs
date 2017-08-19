using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soldier.Head
{
    class Flip : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        public void SetLookAtLeft()
        {
            spriteRenderer.flipY = true;
        }

        public void SetLookAtRight()
        {
            spriteRenderer.flipY = false;
        }
    }
}