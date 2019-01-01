using BinaryProcessing;
using System;

namespace Server
{
    public class WhenIAmRoomOwnerMessageReceivingEvent
    {
        public WhenIAmRoomOwnerMessageReceivingEvent(Binary data, Guid fromUserId)
        {
            FromUserId = fromUserId;
            Data = data;
        }

        public Guid FromUserId { get; private set; }

        public Binary Data { get; private set; }
    }
}
