using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soldier.Rotation
{
    public class LookEvent
    {
        public LookEvent(string session, Vector2 position, LookDirection direction)
        {
            Session = session;
            Position = position;
            Direction = direction;
        }

        public string Session { get; private set; }

        public Vector2 Position { get; private set; }

        public LookDirection Direction { get; private set; }
    }
}