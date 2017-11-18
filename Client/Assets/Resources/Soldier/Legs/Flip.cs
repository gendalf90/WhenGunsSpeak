using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soldier.Legs
{
    class Flip : MonoBehaviour
    {
        private Transform legsTransform;

        private void Awake()
        {
            legsTransform = transform.Find("Legs");
        }

        public void ToLeft()
        {
            legsTransform.SetFlipX(true);
        }

        public void ToRight()
        {
            legsTransform.SetFlipX(false);
        }
    }
}