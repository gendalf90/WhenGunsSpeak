using System;
using System.Collections.Generic;

namespace Server
{
    public class OnAllRoomsUpdatedEvent
    {
        public OnAllRoomsUpdatedEvent(IEnumerable<RoomShortData> allRooms)
        {
            AllRooms = allRooms;
        }

        public IEnumerable<RoomShortData> AllRooms { get; }
    }

    public class RoomShortData
    {
        public RoomShortData(Guid ownerId, string header)
        {
            OwnerId = ownerId;
            Header = header;
        }

        public Guid OwnerId { get; }

        public string Header { get; }
    }
}
