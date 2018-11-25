using System;

namespace Server
{
    class UserWantsToJoinToRoomEvent
    {
        public UserWantsToJoinToRoomEvent(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; private set; }
    }
}
