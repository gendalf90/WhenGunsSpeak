using System;

namespace Server
{
    class IAmNotJoinedToRoomEvent
    {
        public IAmNotJoinedToRoomEvent(Guid roomOwnerId, string message)
        {
            RoomOwnerId = roomOwnerId;
            Message = message;
        }

        public Guid RoomOwnerId { get; private set; }

        public string Message { get; private set; }
    }
}
