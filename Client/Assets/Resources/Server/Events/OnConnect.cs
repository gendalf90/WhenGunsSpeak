using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    public class OnConnectEvent
    {
        public OnConnectEvent(Guid serverId)
        {
            ServerId = serverId;
        }

        public Guid ServerId { get; private set; }
    }
}
