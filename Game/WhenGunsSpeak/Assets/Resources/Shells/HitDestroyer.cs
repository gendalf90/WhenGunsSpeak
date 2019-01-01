using Messages;
using System;
using UnityEngine;

namespace Shells
{
    class HitDestroyer : MonoBehaviour
    {
        private Observable observable;

        public Guid ShellGuid { get; set; }

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
        }

        private void OnEnable()
        {
            observable.Subscribe<OnTargetHitEvent>(TargetHitHandle);
        }

        private void OnDisable()
        {
            observable.Unsubscribe<OnTargetHitEvent>(TargetHitHandle);
        }

        private void TargetHitHandle(OnTargetHitEvent e)
        {
            if (e.ShellId == ShellGuid)
            {
                Destroy(gameObject);
            }
        }
    }
}
