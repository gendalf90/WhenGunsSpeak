using Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soldier.Ground
{
    class Checker : MonoBehaviour
    {
        private Observable observable;

        private bool grounded;

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
        }

        public string Session { get; set; }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if(grounded)
            {
                return;
            }

            grounded = true;
            observable.Publish(new GroundEvent(Session, grounded));
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            grounded = false;
            observable.Publish(new GroundEvent(Session, grounded));
        }
    }
}