using System;

namespace Soldier
{
    public class GroundingEvent
    {
        public GroundingEvent(Guid playerGuid, bool status)
        {
            PlayerGuid = playerGuid;
            IsGrounded = status;
        }

        public Guid PlayerGuid { get; private set; }

        public bool IsGrounded { get; private set; }
    }
}