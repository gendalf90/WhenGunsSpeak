using System;

namespace Server
{
    class StartMessagingCommand
    {
        public StartMessagingCommand(Guid withUserId)
        {
            WithUserId = withUserId;
        }

        public Guid WithUserId { get; private set; }
    }
}
