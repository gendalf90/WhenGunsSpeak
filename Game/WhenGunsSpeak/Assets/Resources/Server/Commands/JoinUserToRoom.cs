using System;

namespace Server
{
    class JoinUserToRoomCommand
    {
        public JoinUserToRoomCommand(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; private set; }
    }
}
