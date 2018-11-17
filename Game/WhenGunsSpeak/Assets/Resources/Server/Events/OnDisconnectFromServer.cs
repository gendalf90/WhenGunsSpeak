using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    public class OnDisconnectFromServerEvent
    {
        public OnDisconnectFromServerEvent(string serverSession)
        {
            ServerSession = serverSession;
        }

        public string ServerSession { get; private set; }
    }
}
