using System;

namespace Connection
{
    public sealed class MessagingStartedData
    {
        public Guid UserId { get; set; }

        public byte[] SecurityKey { get; set; }
    }
}
