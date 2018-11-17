using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    public class OnPacketsEvent
    {
        public OnPacketsEvent(IEnumerable<IPacket> packets)
        {
            Packets = packets;
        }

        public IEnumerable<IPacket> Packets { get; private set; }
    }
}