using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    public class OnDisconnectionEvent
    {
        public OnDisconnectionEvent(string session)
        {
            Session = session;
        }

        public string Session { get; private set; }
    }
}

