using System;

namespace Menu.Multiplayer
{
    class RoomIsSelectedEvent
    {
        public RoomIsSelectedEvent(Guid ownerId)
        {
            OwnerId = ownerId;
        }

        public Guid OwnerId { get; private set; }
    }
}
