using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    public class OnClientConnectEvent
    {
        public OnClientConnectEvent(string session)
        {
            Session = session;
        }

        public string Session { get; private set; }
    }
}