using System;

namespace Soldier
{
    class SoldierFlipEvent
    {
        public SoldierFlipEvent(Guid playerGuid, LookDirection lookDirection)
        {
            LookDirection = lookDirection;
            PlayerGuid = playerGuid;
        }

        public Guid PlayerGuid { get; private set; }

        public LookDirection LookDirection { get; private set; }
    }
}
