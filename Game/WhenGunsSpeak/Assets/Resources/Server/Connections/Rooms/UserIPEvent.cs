using System;
using System.Net;

namespace Server
{
    class UserIPEvent
    {
        public UserIPEvent(Guid userId, IPEndPoint userIp)
        {
            UserId = userId;
            UserIp = userIp;
        }

        public Guid UserId { get; private set; }

        public IPEndPoint UserIp { get; private set; }
    }
}
