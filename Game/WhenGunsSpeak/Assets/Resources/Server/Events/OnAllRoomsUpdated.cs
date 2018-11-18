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
}
