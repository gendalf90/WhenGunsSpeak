using Messages;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    class StoppingState : MonoBehaviour
    {
        private Observable observable;
        private Udp udp;
        private InactiveState inactive;

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
            udp = GetComponent<Udp>();
            inactive = GetComponent<InactiveState>();
        }

        private void Update()
        {
            ReleaseUdp();
            NotifyThatHasStopped();
            StartInactiveState();
            Disable();
        }

        private void StartInactiveState()
        {
            inactive.enabled = true;
        }

        private void NotifyThatHasStopped()
        {
            observable.Publish(new OnStoppedEvent());
        }

        private void ReleaseUdp()
        {
            udp.enabled = false;
        }

        private void Disable()
        {
            enabled = false;
        }
    }
}
