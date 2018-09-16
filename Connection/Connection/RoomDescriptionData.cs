using System;

namespace Connection
{
    public sealed class RoomDescriptionData
    {
        public Guid RoomId { get; set; }

        public Guid OwnerId { get; set; }

        public string Description { get; set; }
    }
}
