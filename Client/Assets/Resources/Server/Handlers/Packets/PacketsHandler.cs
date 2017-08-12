using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    class PacketsHandler : MonoBehaviour
    {
        private Udp udp;

        private void Awake()
        {
            udp = FindObjectOfType<Udp>();
        }

        private void Update()
        {
            var packets = udp.Receive();

            if (packets.Length > 0)
            {
                OnReceive.SafeRaise(this, new ReceivePacketsEventArgs(packets));
            }
        }

        public event EventHandler<ReceivePacketsEventArgs> OnReceive;
    }
}
