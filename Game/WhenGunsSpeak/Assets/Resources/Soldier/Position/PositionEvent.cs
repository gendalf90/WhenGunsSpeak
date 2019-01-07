using System;
using UnityEngine;

namespace Soldier
{
    class PositionEvent
    {
        public PositionEvent(Guid playerGuid, Vector2 position)
        {
            PlayerGuid = playerGuid;
            Position = position;
        }

        public Guid PlayerGuid { get; private set; }

        public Vector2 Position { get; private set; }
    }
}
