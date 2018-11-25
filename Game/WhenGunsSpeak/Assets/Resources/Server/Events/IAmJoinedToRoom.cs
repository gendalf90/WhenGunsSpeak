using System;

namespace Server
{
    public class IAmJoinedToRoomEvent
    {
        public IAmJoinedToRoomEvent(Guid roomOwnerId)
        {
            RoomOwnerId = roomOwnerId;
        }

        public Guid RoomOwnerId { get; private set; }
    }
}
