using Messages;
using System;
using UnityEngine;

namespace Soldier
{
    class GroundingChecker : MonoBehaviour
    {
        private Observable observable;

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
        }

        public Guid PlayerGuid { get; set; }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            observable.Publish(new GroundingEvent(PlayerGuid, true));
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            observable.Publish(new GroundingEvent(PlayerGuid, false));
        }
    }
}