using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soldier.Rotation
{
    public class LookCommand
    {
        public LookCommand(string session, Vector2 position)
        {
            Session = session;
            Position = position;
        }

        public string Session { get; private set; }

        public Vector2 Position { get; private set; }
    }
}