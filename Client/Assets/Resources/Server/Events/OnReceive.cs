using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    public class OnReceiveEvent
    {
        public OnReceiveEvent(Guid from, IEnumerable<IPacket> data)
        {
            From = from;
            Data = data;
        }

        public Guid From { get; private set; }

        public IEnumerable<IPacket> Data { get; private set; }
    }
}