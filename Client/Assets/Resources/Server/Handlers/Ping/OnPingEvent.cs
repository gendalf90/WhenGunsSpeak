using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    public class OnPingEvent
    {
        public OnPingEvent(string fromSession)
        {
            FromSession = fromSession;
        }

        public string FromSession { get; private set; }
    }
}