using System;

namespace Connection
{
    public sealed class RoomRejectingData
    {
        public Guid RoomId { get; set; }

        public string Message { get; set; }
    }
}
