using Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    class PacketsHandler : MonoBehaviour
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
            var packets = udp.Receive();

            if (packets.Length > 0)
            {
                observable.Publish(new OnPacketsEvent(packets));
            }
        }
    }
}
