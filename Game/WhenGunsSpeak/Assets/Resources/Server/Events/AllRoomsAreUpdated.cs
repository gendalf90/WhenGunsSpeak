using System.Collections.Generic;

namespace Server
{
    public class AllRoomsAreUpdatedEvent
    {
        public AllRoomsAreUpdatedEvent(IEnumerable<RoomShortData> allRooms)
        {
            AllRooms = allRooms;
        }

        public IEnumerable<RoomShortData> AllRooms { get; }
    }
}
