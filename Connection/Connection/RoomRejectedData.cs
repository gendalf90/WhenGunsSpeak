using System;

namespace Connection
{
    public sealed class RoomRejectedData
    {
        public Guid OwnerId { get; set; }

        public string Message { get; set; }
    }
}
