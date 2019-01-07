using System;
using UnityEngine;

namespace Soldier
{
    class SoldierLookingEvent
    {
        public SoldierLookingEvent(Guid playerGuid, Vector2 lookingPosition)
        {
            PlayerGuid = playerGuid;
            LookingPosition = lookingPosition;
        }

        public Guid PlayerGuid { get; private set; }

        public Vector2 LookingPosition { get; private set; }
    }
}
