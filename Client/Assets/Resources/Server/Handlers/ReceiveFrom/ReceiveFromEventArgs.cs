using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    class ReceiveFromEventArgs : EventArgs
    {
        public ReceiveFromEventArgs(Guid from, IEnumerable<IPacket> data)
        {
            From = from;
            Data = data;
        }

        public Guid From { get; private set; }

        public IEnumerable<IPacket> Data { get; private set; }
    }
}
