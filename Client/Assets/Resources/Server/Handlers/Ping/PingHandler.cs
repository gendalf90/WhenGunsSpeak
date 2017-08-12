using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Server
{
    class PingHandler : MonoBehaviour
    {
        private PacketsHandler packetsHandler;

        private void Awake()
        {
            packetsHandler = GetComponent<PacketsHandler>();
        }

        private void Start()
        {
            packetsHandler.OnReceive += Receive;
        }

        private void Receive(object sender, ReceivePacketsEventArgs args)
        {
            args.Packets.Select(AsPing)
                        .Where(ping => ping != null)
                        .Select(ping => ping.Initiator)
                        .Distinct()
                        .Select(from => new ReceivePingEventArgs(from))
                        .ForEach(pingArgs => OnPingReceive.SafeRaise(this, pingArgs));
        }

        private Ping AsPing(IPacket packet)
        {
            using (var reader = packet.ToReader())
            {
                var header = reader.ReadFromJson<Ping>();

                if (header.Action == "ping")
                {
                    return header;
                }

                return null;
            }
        }

        public event EventHandler<ReceivePingEventArgs> OnPingReceive;

        private void OnDestroy()
        {
            packetsHandler.OnReceive -= Receive;
        }

        class Ping
        {
            public string Action { get; set; }

            public Guid Initiator { get; set; }
        }
    }
}
