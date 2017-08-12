using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    public class OnClientDisconnectEvent
    {
        public OnClientDisconnectEvent(Guid guid)
        {
            Guid = guid;
        }

        public Guid Guid { get; private set; }
    }
}
