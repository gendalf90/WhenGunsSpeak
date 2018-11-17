using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    public class OnReceiveEvent
    {
        public OnReceiveEvent(string fromSession, IEnumerable<IPacket> data)
        {
            FromSession = fromSession;
            Data = data;
        }

        public string FromSession { get; private set; }

        public IEnumerable<IPacket> Data { get; private set; }
    }
}