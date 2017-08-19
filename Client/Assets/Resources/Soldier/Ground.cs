using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soldier
{
    public class Ground : MonoBehaviour
    {
        public bool IsIntersect { get; private set; }

        private void OnTriggerStay2D(Collider2D collision)
        {
            IsIntersect = true;
        }
        
        private void OnTriggerExit2D(Collider2D collision)
        {
            IsIntersect = false;
        }
    }
}