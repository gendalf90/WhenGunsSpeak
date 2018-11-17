using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    public class OnReceiveFromServerEvent
    {
        public OnReceiveFromServerEvent(IEnumerable<IPacket> data)
        {
            Data = data;
        }

        public IEnumerable<IPacket> Data { get; private set; }
    }
}