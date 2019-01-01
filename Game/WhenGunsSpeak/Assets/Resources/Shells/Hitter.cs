using Messages;
using System;
using UnityEngine;

namespace Shells
{
    class Hitter : MonoBehaviour
    {
        private Observable observable;

        public Guid ShellGuid { get; set; }

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            observable.Publish(new OnShellHitEvent(ShellGuid, other.GetInstanceID()));
        }
    }
}
