using System;

namespace Server
{
    public class IAmJoinedToRoomEvent
    {
        public IAmJoinedToRoomEvent(Guid roomOwnerId, Guid myId)
        {
            MyId = myId;
            RoomOwnerId = roomOwnerId;
        }

        public Guid MyId { get; private set; }

        public Guid RoomOwnerId { get; private set; }
    }
}
