using System;

namespace Soldier
{
    public class StartLegsAnimationCommand
    {
        public StartLegsAnimationCommand(Guid playerGuid, LegsAnimationType type)
        {
            PlayerGuid = playerGuid;
            Type = type;
        }

        public Guid PlayerGuid { get; private set; }

        public LegsAnimationType Type { get; private set; }
    }
}