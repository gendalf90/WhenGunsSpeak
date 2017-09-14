using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    public class OnRoomsEvent
    {
        public OnRoomsEvent(IEnumerable<RoomItem> rooms)
        {
            Rooms = rooms;
        }

        public IEnumerable<RoomItem> Rooms { get; private set; }
    }
}