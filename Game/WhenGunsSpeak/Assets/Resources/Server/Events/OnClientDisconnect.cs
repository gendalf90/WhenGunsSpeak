using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    public class OnClientDisconnectEvent
    {
        public OnClientDisconnectEvent(string session)
        {
            Session = session;
        }

        public string Session { get; private set; }
    }
}
