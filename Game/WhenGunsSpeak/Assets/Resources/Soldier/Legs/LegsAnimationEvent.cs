using System;

namespace Soldier
{
    class LegsAnimationEvent
    {
        public LegsAnimationEvent(Guid playerGuid, LegsAnimationType type)
        {
            PlayerGuid = playerGuid;
            Type = type;
        }

        public Guid PlayerGuid { get; private set; }

        public LegsAnimationType Type { get; private set; }
    }
}
