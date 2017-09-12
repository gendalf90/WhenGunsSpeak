using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    public class OnConnectToServerEvent
    {
        public OnConnectToServerEvent(string serverSession)
        {
            ServerSession = serverSession;
        }

        public string ServerSession { get; private set; }
    }
}
