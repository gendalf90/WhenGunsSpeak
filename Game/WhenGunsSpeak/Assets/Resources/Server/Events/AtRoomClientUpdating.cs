using System;

namespace Server
{
    class AtRoomClientUpdatingEvent
    {
        public AtRoomClientUpdatingEvent(Guid myId)
        {
            MyId = myId;
        }

        public Guid MyId { get; private set; }
    }
}
