using System;

namespace Server
{
    class UserIsJoiningToRoomEvent
    {
        public UserIsJoiningToRoomEvent(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; private set; }
    }
}
