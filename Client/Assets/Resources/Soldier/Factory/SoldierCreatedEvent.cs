using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soldier.Factory
{
    public class SoldierCreatedEvent
    {
        public SoldierCreatedEvent(string session)
        {
            Session = session;
        }

        public string Session { get; private set; }
    }
}