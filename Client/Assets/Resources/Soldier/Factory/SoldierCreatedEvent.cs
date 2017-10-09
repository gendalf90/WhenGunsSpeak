using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soldier.Factory
{
    public class SoldierCreatedEvent
    {
        public SoldierCreatedEvent(Guid guid)
        {
            Guid = guid;
        }

        public Guid Guid { get; private set; }
    }
}