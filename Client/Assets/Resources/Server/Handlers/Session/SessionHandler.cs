using Messages;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Server
{
    class SessionHandler : MonoBehaviour
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
            e.Packets.Select(AsSessionHeader)
                     .Where(header => header != null)
                     .Select(header => header.Session)
                     .Distinct()
                     .Select(session => new OnSessionEvent(session))
                     .ForEach(observable.Publish);
        }

        private SessionHeader AsSessionHeader(IPacket packet)
        {
            using (var reader = packet.ToReader())
            {
                return reader.ReadFromJson<SessionHeader>().If(header => header.Action == "session");
            }
        }

        class SessionHeader
        {
            public string Action { get; set; }

            public string Session { get; set; }
        }
    }
}