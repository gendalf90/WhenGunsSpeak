using Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Server
{
    class RoomsHandler : MonoBehaviour
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
            var items = e.Packets.Select(AsRoomsHeader)
                                 .Where(header => header != null)
                                 .SelectMany(header => header.Rooms)
                                 .Distinct()
                                 .Select(pair => new RoomItem(pair.Key, pair.Value))
                                 .ToList();
            observable.Publish(new OnRoomsEvent(items));
        }

        private RoomsHeader AsRoomsHeader(IPacket packet)
        {
            using (var reader = packet.ToReader())
            {
                return reader.ReadFromJson<RoomsHeader>().If(header => header.Action == "rooms");
            }
        }

        class RoomsHeader
        {
            public string Action { get; set; }

            public Dictionary<string, string> Rooms { get; set; }
        }
    }
}
