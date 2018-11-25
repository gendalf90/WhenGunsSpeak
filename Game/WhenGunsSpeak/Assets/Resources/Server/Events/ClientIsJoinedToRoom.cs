using System;

namespace Server
{
    public class ClientIsJoinedToRoomEvent
    {
        public ClientIsJoinedToRoomEvent(Guid clientId)
        {
            ClientId = clientId;
        }

        public Guid ClientId { get; private set; }
    }
}