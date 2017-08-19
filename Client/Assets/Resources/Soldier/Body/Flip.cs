using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soldier.Body
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
            spriteRenderer.flipX = true;
        }

        public void SetLookAtRight()
        {
            spriteRenderer.flipX = false;
        }
    }
}