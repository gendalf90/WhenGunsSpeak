using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    public class OnStartedAsServerEvent
    {
        public OnStartedAsServerEvent(string session)
        {
            MySession = session;
        }

        public string MySession { get; private set; }
    }
}
