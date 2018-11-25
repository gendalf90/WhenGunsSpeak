using System;

namespace Server
{
    class StartUserToRoomJoiningCommand
    {
        public StartUserToRoomJoiningCommand(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; private set; }
    }
}
