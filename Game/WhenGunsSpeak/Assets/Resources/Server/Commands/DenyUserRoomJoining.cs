using System;

namespace Server
{
    class DenyUserRoomJoiningCommand
    {
        public DenyUserRoomJoiningCommand(Guid userId, string message)
        {
            UserId = userId;
            Message = message;
        }

        public Guid UserId { get; private set; }

        public string Message { get; private set; }
    }
}
