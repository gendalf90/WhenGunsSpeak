using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soldier
{
    public class RemoveSoldierCommand
    {
        public RemoveSoldierCommand(Guid guid)
        {
            Guid = guid;
        }

        public Guid Guid { get; private set; }
    }
}