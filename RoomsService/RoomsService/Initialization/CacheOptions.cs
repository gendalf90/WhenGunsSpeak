using System;

namespace RoomsService.Initialization
{
    public class CacheOptions
    {
        public TimeSpan? AllRoomsExpiration { get; set; }

        public TimeSpan? RoomExpiration { get; set; }
    }
}
