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

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
            udp = FindObjectOfType<Udp>();
        }

        private void Update()
        {
            ReleaseUdp();
            NotifyThatHasStopped();
            StartInactiveState();
            Destroy(gameObject);
        }

        private void StartInactiveState()
        {
            Instantiate(Resources.Load<GameObject>("Server/States/Inactive"));
        }

        private void NotifyThatHasStopped()
        {
            observable.Publish(new OnStoppedEvent());
        }

        private void ReleaseUdp()
        {
            Destroy(udp.gameObject);
        }
    }
}
