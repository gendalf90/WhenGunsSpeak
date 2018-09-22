using System;

namespace Connection
{
    public sealed class RoomRejectedData
    {
        public Guid RoomId { get; set; }

        public string Message { get; set; }
    }
}
