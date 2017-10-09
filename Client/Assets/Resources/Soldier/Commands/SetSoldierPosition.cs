using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soldier
{
    public class SetSoldierPositionCommand
    {
        public SetSoldierPositionCommand(Guid guid, Vector2 position)
        {
            Guid = guid;
            Position = position;
        }

        public Guid Guid { get; private set; }

        public Vector2 Position { get; private set; }
    }
}