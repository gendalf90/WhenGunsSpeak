using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Server
{
    class ReceiveFromHandler : MonoBehaviour
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
            args.Packets.Select(AsReceivedData)
                        .Where(data => data != null)
                        .GroupBy(data => data.From, data => data.Packet)
                        .Select(group => new ReceiveFromEventArgs(group.Key, group))
                        .ForEach(fromArgs => OnReceiveFrom.SafeRaise(this, fromArgs));
        }

        private ReceivedData AsReceivedData(IPacket packet)
        {
            using (var reader = packet.ToReader())
            {
                var metadata = reader.ReadFromJson<ReceivedDataHeader>();

                if (metadata.Action != "sendto")
                {
                    return null;
                }

                return new ReceivedData { From = metadata.From, Packet = reader.ReadBytes().ToPacket() };
            }
        }

        public event EventHandler<ReceiveFromEventArgs> OnReceiveFrom;

        private void OnDestroy()
        {
            packetsHandler.OnReceive -= Receive;
        }

        class ReceivedDataHeader
        {
            public string Action { get; set; }

            public Guid From { get; set; }
        }

        class ReceivedData
        {
            public Guid From { get; set; }

            public IPacket Packet { get; set; }
        }
    }
}
