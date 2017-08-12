using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    public class OnStartedEvent
    {
        public OnStartedEvent(Guid guid, Role role)
        {
            MyGuid = guid;
            MyRole = role;
        }

        public Role MyRole { get; private set; }

        public Guid MyGuid { get; private set; }
    }
}
