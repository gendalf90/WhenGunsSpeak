using BinaryProcessing;
using System;

namespace Server
{
    public class MessageIsReceivedEvent
    {
        public MessageIsReceivedEvent(Binary data, Guid fromUserId)
        {
            FromUserId = fromUserId;
            Data = data;
        }

        public Guid FromUserId { get; private set; }

        public Binary Data { get; private set; }
    }
}
