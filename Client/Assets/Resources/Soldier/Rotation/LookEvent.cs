using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soldier.Rotation
{
    public class LookEvent
    {
        public LookEvent(Guid guid, Vector2 position, LookDirection direction)
        {
            Guid = guid;
            Position = position;
            Direction = direction;
        }

        public Guid Guid { get; private set; }

        public Vector2 Position { get; private set; }

        public LookDirection Direction { get; private set; }
    }
}