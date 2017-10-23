using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soldier.Factory
{
    public class SoldierRemovedEvent
    {
        public SoldierRemovedEvent(string session)
        {
            Session = session;
        }

        public string Session { get; private set; }
    }
}