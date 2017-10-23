using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Soldier.Motion
{
    class PositionEvent
    {
        public PositionEvent(string session, Vector2 position)
        {
            Session = session;
            Position = position;
        }

        public string Session { get; private set; }

        public Vector2 Position { get; private set; }
    }
}
