using System;

namespace Soldier
{
    class SoldierMovingEvent
    {
        public SoldierMovingEvent(Guid playerGuid, bool isRightMoving, bool isLeftMoving, bool isJumping)
        {
            IsRightMoving = isRightMoving;
            IsLeftMoving = isLeftMoving;
            IsJumping = isJumping;
            PlayerGuid = playerGuid;
        }

        public bool IsRightMoving { get; private set; }

        public bool IsLeftMoving { get; private set; }

        public bool IsJumping { get; private set; }

        public Guid PlayerGuid { get; private set; }
    }
}
