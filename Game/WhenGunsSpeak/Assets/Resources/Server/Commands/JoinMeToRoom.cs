using System;

namespace Server
{
    class JoinMeToRoomCommand
    {
        public JoinMeToRoomCommand(Guid roomOwnerId)
        {
            RoomOwnerId = roomOwnerId;
        }

        public Guid RoomOwnerId { get; private set; }
    }
}
