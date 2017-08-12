using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    public class OnClientConnectEvent
    {
        public OnClientConnectEvent(Guid guid)
        {
            Guid = guid;
        }

        public Guid Guid { get; private set; }
    }
}