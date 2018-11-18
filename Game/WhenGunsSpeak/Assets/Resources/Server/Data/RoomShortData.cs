using System;

namespace Server
{
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
