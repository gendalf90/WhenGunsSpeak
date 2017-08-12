using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    class ReceivePacketsEventArgs : EventArgs
    {
        public ReceivePacketsEventArgs(IEnumerable<IPacket> packets)
        {
            Packets = packets;
        }

        public IEnumerable<IPacket> Packets { get; private set; }
    }
}
