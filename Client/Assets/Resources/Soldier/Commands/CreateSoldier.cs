using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soldier
{
    public class CreateSoldierCommand
    {
        public CreateSoldierCommand(Guid guid)
        {
            Guid = guid;
        }

        public Guid Guid { get; private set; }
    }
}