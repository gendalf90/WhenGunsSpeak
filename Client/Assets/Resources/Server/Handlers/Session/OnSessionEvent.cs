using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    public class OnSessionEvent
    {
        public OnSessionEvent(string session)
        {
            Session = session;
        }

        public string Session { get; private set; }
    }
}