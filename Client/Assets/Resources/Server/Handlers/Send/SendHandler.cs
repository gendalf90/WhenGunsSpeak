using Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Server
{
    class SendHandler : MonoBehaviour
    {
        private Observable observable;

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
        }

        private void Start()
        {
            observable.Subscribe<OnPacketsEvent>(Receive);
        }

        private void OnDestroy()
        {
            observable.Unsubscribe<OnPacketsEvent>(Receive);
        }

        private void Receive(OnPacketsEvent e)
        {
            e.Packets.Select(AsSendData)
                     .Where(data => data != null)
                     .GroupBy(data => data.From, data => data.Packet)
                     .Select(group => new OnSendEvent(group.Key, group))
                     .ForEach(observable.Publish);
        }

        private SendData AsSendData(IPacket packet)
        {
            using (var reader = packet.ToReader())
            {
                return reader.ReadFromJson<SendHeader>()
                             .If(header => header.Action == "send")
                             .Return(header => new SendData { From = header.From, Packet = reader.ReadBytes().ToPacket() });
            }
        }

        class SendHeader
        {
            public string Action { get; set; }

            public string From { get; set; }
        }

        class SendData
        {
            public string From { get; set; }

            public IPacket Packet { get; set; }
        }
    }
}
