using Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Server
{
    class PingHandler : MonoBehaviour
    {
        private Observable observable;

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
        }

        private void OnEnable()
        {
            observable.Subscribe<OnPacketsEvent>(Receive);
        }

        private void OnDisable()
        {
            observable.Unsubscribe<OnPacketsEvent>(Receive);
        }

        private void Receive(OnPacketsEvent e)
        {
            e.Packets.Select(AsPingHeader)
                     .Where(header => header != null)
                     .Select(header => header.From)
                     .Distinct()
                     .Select(from => new OnPingEvent(from))
                     .ForEach(observable.Publish);
        }

        private PingHeader AsPingHeader(IPacket packet)
        {
            using (var reader = packet.ToReader())
            {
                return reader.ReadFromJson<PingHeader>().If(header => header.Action == "ping");
            }
        }

        class PingHeader
        {
            public string Action { get; set; }

            public string From { get; set; }
        }
    }
}
