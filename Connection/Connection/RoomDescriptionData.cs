using System;

namespace Connection
{
    public sealed class RoomDescriptionData
    {
        public Guid OwnerId { get; set; }

        public string Header { get; set; }

        public string Description { get; set; }
    }
}
