using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    public class OnStartedAsClientEvent
    {
        public OnStartedAsClientEvent(string session)
        {
            MySession = session;
        }

        public string MySession { get; private set; }
    }
}
