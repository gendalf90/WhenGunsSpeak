using System;

namespace Server
{
    public class RefreshRoomCommand
    {
        public RefreshRoomCommand(Guid ownerId)
        {
            OwnerId = ownerId;
        }

        public Guid OwnerId { get; }
    }
}
