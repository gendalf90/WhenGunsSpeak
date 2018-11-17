using System;

namespace Server
{
    public class OnRoomUpdatedEvent
    {
        public OnRoomUpdatedEvent(Guid ownerId, string header, string description)
        {
            OwnerId = ownerId;
            Header = header;
            Description = description;
        }

        public Guid OwnerId { get; }

        public string Header { get; }

        public string Description { get; }
    }
}
